package com.example.apz_mobile.ui.subordinate

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.apz_mobile.AddableFragment
import com.example.apz_mobile.R
import com.example.apz_mobile.databinding.FragmentSubordinatesBinding
import com.example.apz_mobile.ui.sensors.SensorsFragment

class SubordinatesFragment : Fragment(), AddableFragment {

    private var _binding: FragmentSubordinatesBinding? = null
    private val binding get() = _binding!!
    private lateinit var subordinatesViewModel: SubordinatesViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentSubordinatesBinding.inflate(inflater, container, false)
        val root: View = binding.root

        val factory = SubordinatesViewModelFactory(requireContext())
        subordinatesViewModel = ViewModelProvider(this, factory).get(SubordinatesViewModel::class.java)

        val recyclerView = binding.recyclerViewSubordinates
        recyclerView.layoutManager = LinearLayoutManager(context)

        subordinatesViewModel.subordinates.observe(viewLifecycleOwner) { subordinates ->
            if (subordinates != null) {
                recyclerView.adapter = SubordinatesAdapter(subordinates) { subordinate ->
                    val intent = Intent(requireContext(), SubordinateDetailActivity::class.java)
                    intent.putExtra("subordinate", subordinate)
                    startActivityForResult(intent, SUBORDINATE_DETAIL_REQUEST_CODE)
                }
            } else {
                Toast.makeText(context, "Failed to load subordinates", Toast.LENGTH_SHORT).show()
            }
        }
        loadSubordinates()
        binding.swipeRefreshLayout.setOnRefreshListener {
            loadSubordinates()
            binding.swipeRefreshLayout.isRefreshing = false
        }
        return root
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        (activity as? AppCompatActivity)?.supportActionBar?.title = getString(R.string.nav_subordinates)
    }
    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == SUBORDINATE_DETAIL_REQUEST_CODE && resultCode == Activity.RESULT_OK) {
            loadSubordinates()
        }
    }

    private fun loadSubordinates() {
        subordinatesViewModel.refreshSubordinates(requireContext())
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onAddButtonClicked() {
        val intent = Intent(requireContext(), RegisterSubordinateActivity::class.java)
        startActivity(intent)
    }

    companion object {
        private const val SUBORDINATE_DETAIL_REQUEST_CODE = 1
    }
}
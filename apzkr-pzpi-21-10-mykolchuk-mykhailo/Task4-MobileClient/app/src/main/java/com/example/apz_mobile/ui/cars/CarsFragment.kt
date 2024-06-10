package com.example.apz_mobile.ui.cars

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
import com.example.apz_mobile.databinding.FragmentCarsBinding

class CarsFragment : Fragment(), AddableFragment {

    private var _binding: FragmentCarsBinding? = null
    private val binding get() = _binding!!
    private lateinit var carsViewModel: CarsViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentCarsBinding.inflate(inflater, container, false)
        val root: View = binding.root

        val factory = CarsViewModelFactory(requireContext())
        carsViewModel = ViewModelProvider(this, factory).get(CarsViewModel::class.java)

        val recyclerView = binding.recyclerViewCars
        recyclerView.layoutManager = LinearLayoutManager(context)

        carsViewModel.cars.observe(viewLifecycleOwner) { cars ->
            if (cars != null) {
                recyclerView.adapter = CarsAdapter(requireContext(), cars) { car ->
                    val intent = Intent(requireContext(), CarDetailActivity::class.java)
                    intent.putExtra("car", car)
                    startActivity(intent)
                }
            } else {
                Toast.makeText(context, "Failed to load cars", Toast.LENGTH_SHORT).show()
            }
        }

        binding.swipeRefreshLayout.setOnRefreshListener {
            carsViewModel.refreshCars(requireContext())
            binding.swipeRefreshLayout.isRefreshing = false
        }
        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        (activity as? AppCompatActivity)?.supportActionBar?.title = getString(R.string.nav_cars)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onAddButtonClicked() {
        val intent = Intent(requireContext(), AddCarActivity::class.java)
        startActivity(intent)
    }
}



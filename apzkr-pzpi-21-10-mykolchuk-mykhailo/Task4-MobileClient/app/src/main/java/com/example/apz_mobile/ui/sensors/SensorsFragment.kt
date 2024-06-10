package com.example.apz_mobile.ui.sensors

import android.app.Activity
import android.content.Context
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
import com.example.apz_mobile.databinding.FragmentSensorsBinding
import com.example.apz_mobile.ui.cars.AddCarActivity

class SensorsFragment : Fragment(), AddableFragment {

    private var _binding: FragmentSensorsBinding? = null
    private val binding get() = _binding!!
    private lateinit var sensorsViewModel: SensorsViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentSensorsBinding.inflate(inflater, container, false)
        val root: View = binding.root

        val factory = SensorsViewModelFactory(requireContext())
        sensorsViewModel = ViewModelProvider(this, factory).get(SensorsViewModel::class.java)

        val recyclerView = binding.recyclerViewSensors
        recyclerView.layoutManager = LinearLayoutManager(context)

        sensorsViewModel.sensors.observe(viewLifecycleOwner) { sensors ->
            if (sensors != null) {
                recyclerView.adapter = SensorsAdapter(sensors) { sensor ->
                    val intent = Intent(requireContext(), SensorDetailActivity::class.java)
                    intent.putExtra("sensor", sensor)
                    startActivityForResult(intent, SENSOR_DETAIL_REQUEST_CODE)
                }
            } else {
                Toast.makeText(context, "Failed to load sensors", Toast.LENGTH_SHORT).show()
            }
        }
        binding.swipeRefreshLayout.setOnRefreshListener {
            sensorsViewModel.refreshSensors(requireContext())
            binding.swipeRefreshLayout.isRefreshing = false
        }

        loadSensors()

        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        (activity as? AppCompatActivity)?.supportActionBar?.title = getString(R.string.nav_sensors)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == SENSOR_DETAIL_REQUEST_CODE && resultCode == Activity.RESULT_OK) {
            loadSensors()
        }
    }

    private fun loadSensors() {
        sensorsViewModel.refreshSensors(requireContext())
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onAddButtonClicked() {
        val intent = Intent(requireContext(), AddSensorActivity::class.java)
        startActivity(intent)
    }

    companion object {
        private const val SENSOR_DETAIL_REQUEST_CODE = 1
    }
}


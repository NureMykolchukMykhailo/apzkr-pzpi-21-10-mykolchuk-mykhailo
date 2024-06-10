package com.example.apz_mobile.ui.sensors

import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.ViewModelProvider
import com.example.apz_mobile.databinding.ActivityAddSensorBinding
import androidx.lifecycle.Observer

class AddSensorActivity : AppCompatActivity() {
    private lateinit var binding: ActivityAddSensorBinding
    private lateinit var addSensorViewModel: AddSensorViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityAddSensorBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val factory = AddSensorViewModelFactory(applicationContext)
        addSensorViewModel = ViewModelProvider(this, factory).get(AddSensorViewModel::class.java)

        binding.viewModel = addSensorViewModel
        binding.lifecycleOwner = this

        binding.buttonAddSensor.setOnClickListener {
            val sensorName = binding.editTextSensorName.text.toString()

            if (sensorName.isNotEmpty()) {
                addSensorViewModel.addSensor(sensorName)
            } else {
                Toast.makeText(this, "Please fill in all fields", Toast.LENGTH_SHORT).show()
            }
        }

        addSensorViewModel.success.observe(this, Observer { success ->
            if (success) {
                Toast.makeText(this, "Sensor added successfully", Toast.LENGTH_SHORT).show()
                finish()
            } else {
                Toast.makeText(this, "Failed to add sensor", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
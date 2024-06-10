package com.example.apz_mobile.ui.sensors

import android.app.Activity
import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.apz_mobile.controllers.SensorsController
import com.example.apz_mobile.databinding.ActivitySensorDetailBinding
import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Sensor
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class SensorDetailActivity : AppCompatActivity() {
    private lateinit var binding: ActivitySensorDetailBinding
    private lateinit var apiService: ApiService
    private lateinit var sensorsController: SensorsController
    private var sensor: Sensor? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySensorDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val tokenManager = TokenManager(this)
        apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
        sensorsController = SensorsController(apiService, tokenManager)

        sensor = intent.getParcelableExtra<Sensor>("sensor")

        sensor?.let {
            binding.sensorName.text = it.name
            if (it.car?.id != null) {
                binding.disconnectButton.visibility = View.VISIBLE
            } else {
                binding.carSpinner.visibility = View.VISIBLE
                binding.connectButton.visibility = View.VISIBLE
                loadCarsWithoutSensors()
            }
        }

        binding.connectButton.setOnClickListener {
            val selectedCar = binding.carSpinner.selectedItem as Car
            connectSensorToCar(sensor?.id ?: 0, selectedCar.id)
        }

        binding.disconnectButton.setOnClickListener {
            disconnectSensorFromCar(sensor?.car?.id ?: 0)
        }
    }

    private fun loadCarsWithoutSensors() {
        sensorsController.getCarsWithoutSensors { cars ->
            if (cars != null) {
                val adapter = ArrayAdapter(this, android.R.layout.simple_spinner_item, cars)
                adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                binding.carSpinner.adapter = adapter
            } else {
                Toast.makeText(this, "Failed to load cars", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun connectSensorToCar(sensorId: Int, carId: Int) {
        sensorsController.connectSensorToCar(sensorId, carId) { isSuccess ->
            if (isSuccess) {
                Toast.makeText(this, "Sensor connected to car", Toast.LENGTH_SHORT).show()
                setResult(Activity.RESULT_OK)
                finish()
            } else {
                Toast.makeText(this, "Failed to connect sensor", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun disconnectSensorFromCar(carId: Int) {
        sensorsController.disconnectSensorFromCar(carId) { isSuccess ->
            if (isSuccess) {
                Toast.makeText(this, "Sensor disconnected from car", Toast.LENGTH_SHORT).show()
                setResult(Activity.RESULT_OK)
                finish()
            } else {
                Toast.makeText(this, "Failed to disconnect sensor", Toast.LENGTH_SHORT).show()
            }
        }
    }
}



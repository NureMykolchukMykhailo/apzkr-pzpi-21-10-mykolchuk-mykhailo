package com.example.apz_mobile.ui.cars


import android.graphics.Color
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.apz_mobile.R
import com.example.apz_mobile.controllers.CarsController
import com.example.apz_mobile.databinding.ActivityCarDetailBinding
import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Driver
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import com.example.apz_mobile.ui.trips.TripAdapter


class CarDetailActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCarDetailBinding
    private lateinit var apiService: ApiService
    private lateinit var tripAdapter: TripAdapter
    private lateinit var tokenManager: TokenManager
    private lateinit var carsController: CarsController

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityCarDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        tokenManager = TokenManager(this)
        apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
        carsController = CarsController(apiService, tokenManager)

        val car = intent.getParcelableExtra<Car>("car")

        car?.let {
            binding.carNameType.text = it.name + " (" + it.type + ")"
            binding.carAdded.text = it.added
            if(it.sensor == null){
                binding.carSensor.text = this.getString(R.string.car_detail_sensor)
                binding.carSensor.setTextColor(Color.RED)
            } else{
                binding.carSensor.text = it.sensor.name
                binding.carSensor.setTextColor(Color.GREEN)
            }
            binding.recyclerViewDrivers.layoutManager = LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL, false)
            binding.recyclerViewDrivers.adapter = DriverAdapter(car.drivers.map { Driver(it.id, it.name, it.email, it.regDate) })
        }

        tripAdapter = TripAdapter(this, emptyList())
        binding.recyclerViewTrips.layoutManager = LinearLayoutManager(this)
        binding.recyclerViewTrips.adapter = tripAdapter

        if (car != null) {
            loadCarTrips(car.id)
        }
    }

    private fun loadCarTrips(carId: Int) {
        carsController.getCarTrips(carId) { trips, isSuccessful ->
            if (isSuccessful) {
                tripAdapter = TripAdapter(this, trips ?: emptyList())
                binding.recyclerViewTrips.adapter = tripAdapter
            } else {
                Toast.makeText(this@CarDetailActivity, "Failed to load trips", Toast.LENGTH_SHORT).show()
            }
        }
    }
}


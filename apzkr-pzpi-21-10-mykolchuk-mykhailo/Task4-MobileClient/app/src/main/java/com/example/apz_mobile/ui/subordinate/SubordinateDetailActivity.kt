package com.example.apz_mobile.ui.subordinate


import android.app.Activity
import android.graphics.Color
import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.apz_mobile.controllers.SubordinatesController
import com.example.apz_mobile.databinding.ActivitySubordinateDetailBinding
import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Subordinate
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager



class SubordinateDetailActivity : AppCompatActivity() {

    private lateinit var binding: ActivitySubordinateDetailBinding
    private lateinit var apiService: ApiService
    private lateinit var controller: SubordinatesController
    private lateinit var tokenManager: TokenManager
    private var subordinate: Subordinate? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySubordinateDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        tokenManager = TokenManager(this)
        apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
        controller = SubordinatesController(apiService, tokenManager)

        subordinate = intent.getParcelableExtra("subordinate")

        subordinate?.let {
            binding.subordinateName.text = it.name
            binding.subordinateEmail.text = it.email
            binding.subordinateRegDate.text = "Register date: ${it.regDate}"
            if (it.car?.id != null) {
                binding.disconnectButton.visibility = View.VISIBLE
                binding.subordinateCarName.text = it.car.name
                binding.subordinateCarType.text = it.car.type
                binding.subordinateCarAdded.text = it.car.added
            } else {
                binding.subordinateCarName.text = "Not connected to car"
                binding.subordinateCarName.setTextColor(Color.RED)
                binding.carSpinner.visibility = View.VISIBLE
                binding.connectButton.visibility = View.VISIBLE
                loadCars()
            }
        }

        binding.connectButton.setOnClickListener {
            val selectedCar = binding.carSpinner.selectedItem as Car
            connectSubordinateToCar(subordinate?.id ?: 0, selectedCar.id)
        }

        binding.disconnectButton.setOnClickListener {
            disconnectSubordinateFromCar(subordinate?.id ?: 0)
        }
    }

    private fun loadCars() {
        controller.getCarsByOwner { cars ->
            if (cars != null) {
                val adapter = ArrayAdapter(this, android.R.layout.simple_spinner_item, cars)
                adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                binding.carSpinner.adapter = adapter
            } else {
                Toast.makeText(this, "Failed to load cars", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun connectSubordinateToCar(subordinateId: Int, carId: Int) {
        controller.connectSubordinateToCar(subordinateId, carId) { isSuccess ->
            if (isSuccess) {
                Toast.makeText(this, "Subordinate connected to car", Toast.LENGTH_SHORT).show()
                setResult(Activity.RESULT_OK)
                finish()
            } else {
                Toast.makeText(this, "Failed to connect subordinate", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun disconnectSubordinateFromCar(subordinateId: Int) {
        controller.disconnectSubordinateFromCar(subordinateId) { isSuccess ->
            if (isSuccess) {
                Toast.makeText(this, "Subordinate disconnected from car", Toast.LENGTH_SHORT).show()
                setResult(Activity.RESULT_OK)
                finish()
            } else {
                Toast.makeText(this, "Failed to disconnect subordinate", Toast.LENGTH_SHORT).show()
            }
        }
    }
}

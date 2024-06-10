package com.example.apz_mobile.ui.cars

import android.os.Bundle
import android.widget.Toast
import androidx.activity.viewModels
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.example.apz_mobile.databinding.ActivityAddCarBinding

class AddCarActivity : AppCompatActivity() {

    private lateinit var binding: ActivityAddCarBinding
    private lateinit var addCarViewModel: AddCarViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityAddCarBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val factory = AddCarViewModelFactory(applicationContext)
        addCarViewModel = ViewModelProvider(this, factory).get(AddCarViewModel::class.java)

        binding.viewModel = addCarViewModel
        binding.lifecycleOwner = this

        binding.buttonAddCar.setOnClickListener {
            val carName = binding.editTextCarName.text.toString()
            val carType = binding.editTextCarType.text.toString()

            if (carName.isNotEmpty() && carType.isNotEmpty()) {
                addCarViewModel.addCar(carName, carType)
            } else {
                Toast.makeText(this, "Please fill in all fields", Toast.LENGTH_SHORT).show()
            }
        }

        addCarViewModel.success.observe(this, Observer { success ->
            if (success) {
                Toast.makeText(this, "Car added successfully", Toast.LENGTH_SHORT).show()
                finish()
            } else {
                Toast.makeText(this, "Failed to add car", Toast.LENGTH_SHORT).show()
            }
        })
    }
}


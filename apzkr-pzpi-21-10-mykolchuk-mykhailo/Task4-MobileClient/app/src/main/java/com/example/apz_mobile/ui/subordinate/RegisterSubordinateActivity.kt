package com.example.apz_mobile.ui.subordinate

import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.ViewModelProvider
import com.example.apz_mobile.databinding.ActivityAddSubordinateBinding
import androidx.lifecycle.Observer

class RegisterSubordinateActivity : AppCompatActivity(){
    private lateinit var binding: ActivityAddSubordinateBinding
    private lateinit var addSubordinateViewModel: RegisterSubordinateViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityAddSubordinateBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val factory = RegisterSubordinateViewModelFactory(applicationContext)
        addSubordinateViewModel = ViewModelProvider(this, factory).get(RegisterSubordinateViewModel::class.java)

        binding.viewModel = addSubordinateViewModel
        binding.lifecycleOwner = this

        binding.buttonAddSub.setOnClickListener {
            val subName = binding.editTextSubName.text.toString()
            val subEmail = binding.editTextSubEmail.text.toString()
            val subPassword = binding.editTextSubPassword.text.toString()

            if (subName.isNotEmpty() && subEmail.isNotEmpty() && subPassword.isNotEmpty()) {
                addSubordinateViewModel.registerSubordinate(subName, subEmail, subPassword)
            } else {
                Toast.makeText(this, "Please fill in all fields", Toast.LENGTH_SHORT).show()
            }
        }

        addSubordinateViewModel.success.observe(this, Observer { success ->
            if (success) {
                Toast.makeText(this, "Subordinate added successfully", Toast.LENGTH_SHORT).show()
                finish()
            } else {
                Toast.makeText(this, "Failed to add subordinate", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
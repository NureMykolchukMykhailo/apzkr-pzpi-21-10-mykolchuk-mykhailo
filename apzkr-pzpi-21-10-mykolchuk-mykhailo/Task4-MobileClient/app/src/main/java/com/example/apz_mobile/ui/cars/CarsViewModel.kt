package com.example.apz_mobile.ui.cars

import android.content.Context
import android.content.Intent
import androidx.lifecycle.*
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import kotlinx.coroutines.launch
import com.example.apz_mobile.LoginActivity
import com.example.apz_mobile.controllers.CarsController
import com.example.apz_mobile.models.Car


class CarsViewModel(context: Context) : ViewModel() {

    private val _cars = MutableLiveData<List<Car>?>()
    val cars: LiveData<List<Car>?> = _cars

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val carsController = CarsController(apiService, tokenManager)

    init {
        loadCars(context)
    }

    private fun loadCars(context: Context) {
        viewModelScope.launch {
            carsController.getCarsByOwner { carList, isAuthorized ->
                if (isAuthorized) {
                    _cars.postValue(carList)
                } else {
                    val intent = Intent(context, LoginActivity::class.java)
                    intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK)
                    context.startActivity(intent)
                }
            }
        }
    }

    fun refreshCars(context: Context) {
        loadCars(context)
    }
}

class CarsViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(CarsViewModel::class.java)) {
            return CarsViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}



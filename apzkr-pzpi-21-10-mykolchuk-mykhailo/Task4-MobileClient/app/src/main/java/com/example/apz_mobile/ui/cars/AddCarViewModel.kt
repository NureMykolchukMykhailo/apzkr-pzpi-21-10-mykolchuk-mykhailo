package com.example.apz_mobile.ui.cars

import android.content.Context
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.apz_mobile.controllers.CarsController
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import kotlinx.coroutines.launch
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class AddCarViewModel(context: Context) : ViewModel() {

    private val _success = MutableLiveData<Boolean>()
    val success: LiveData<Boolean> = _success

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val carsController = CarsController(apiService, tokenManager)

    fun addCar(name: String, type: String) {
        viewModelScope.launch {
            carsController.addCar(name, type) { isSuccess ->
                _success.postValue(isSuccess)
            }
        }
    }
}

class AddCarViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(AddCarViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return AddCarViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}


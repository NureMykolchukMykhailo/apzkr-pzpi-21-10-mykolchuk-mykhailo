package com.example.apz_mobile.ui.sensors

import android.content.Context
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.apz_mobile.controllers.SensorsController
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import com.example.apz_mobile.ui.cars.AddCarViewModel
import kotlinx.coroutines.launch
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class AddSensorViewModel(context: Context) : ViewModel()  {
    private val _success = MutableLiveData<Boolean>()
    val success: LiveData<Boolean> = _success

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val sensorsController = SensorsController(apiService, tokenManager)

    fun addSensor(name: String) {
        viewModelScope.launch {
            sensorsController.addSensor(name) { isSuccess ->
                _success.postValue(isSuccess)
            }
        }
    }
}

class AddSensorViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(AddSensorViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return AddSensorViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

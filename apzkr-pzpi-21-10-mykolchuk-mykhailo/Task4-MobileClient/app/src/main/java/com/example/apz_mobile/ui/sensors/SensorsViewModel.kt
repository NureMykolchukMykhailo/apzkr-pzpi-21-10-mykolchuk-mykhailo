package com.example.apz_mobile.ui.sensors

import android.content.Context
import android.content.Intent
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.apz_mobile.LoginActivity
import com.example.apz_mobile.controllers.SensorsController
import com.example.apz_mobile.models.Sensor
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import kotlinx.coroutines.launch


class SensorsViewModel(context: Context) : ViewModel() {

    private val _sensors = MutableLiveData<List<Sensor>?>()
    val sensors: LiveData<List<Sensor>?> = _sensors

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val sensorsController = SensorsController(apiService, tokenManager)

    init {
        loadSensors(context)
    }

    private fun loadSensors(context: Context) {
        viewModelScope.launch {
            sensorsController.getSensorsByOwner { sensors, isAuthorized ->
                if (isAuthorized) {
                    _sensors.postValue(sensors)
                } else {
                    val intent = Intent(context, LoginActivity::class.java)
                    intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK)
                    context.startActivity(intent)
                }
            }
        }
    }

    fun refreshSensors(context: Context) {
        loadSensors(context)
    }
}

class SensorsViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(SensorsViewModel::class.java)) {
            return SensorsViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}


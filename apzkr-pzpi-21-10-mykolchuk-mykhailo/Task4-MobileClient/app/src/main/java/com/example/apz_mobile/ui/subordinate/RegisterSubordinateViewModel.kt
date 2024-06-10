package com.example.apz_mobile.ui.subordinate

import android.content.Context
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.apz_mobile.controllers.SubordinatesController
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import com.example.apz_mobile.ui.sensors.AddSensorViewModel
import kotlinx.coroutines.launch
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RegisterSubordinateViewModel(context: Context) : ViewModel() {
    private val _success = MutableLiveData<Boolean>()
    val success: LiveData<Boolean> = _success

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val controller = SubordinatesController(apiService, tokenManager)

    fun registerSubordinate(name: String, email: String, password: String) {
        viewModelScope.launch {
            controller.registerSubordinate(name, email, password) { isSuccess ->
                _success.value = isSuccess
            }
        }
    }
}

class RegisterSubordinateViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(RegisterSubordinateViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return RegisterSubordinateViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

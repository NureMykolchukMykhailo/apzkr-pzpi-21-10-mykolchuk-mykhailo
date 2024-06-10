package com.example.apz_mobile.ui.subordinate


import android.content.Context
import android.content.Intent
import androidx.lifecycle.*
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import kotlinx.coroutines.launch

import com.example.apz_mobile.LoginActivity
import com.example.apz_mobile.controllers.SubordinatesController
import com.example.apz_mobile.models.Subordinate


class SubordinatesViewModel(context: Context) : ViewModel() {

    private val _subordinates = MutableLiveData<List<Subordinate>?>()
    val subordinates: LiveData<List<Subordinate>?> = _subordinates

    private val tokenManager = TokenManager(context)
    private val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
    private val controller = SubordinatesController(apiService, tokenManager)

    init {
        loadSubordinates(context)
    }

    private fun loadSubordinates(context: Context) {
        viewModelScope.launch {
            controller.getSubordinatesByChief { subordinates, isSuccess ->
                if (isSuccess) {
                    _subordinates.value = subordinates
                } else {
                    if (subordinates == null) {
                        _subordinates.value = null
                    } else {
                        tokenManager.clearToken()
                        val intent = Intent(context, LoginActivity::class.java)
                        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK)
                        context.startActivity(intent)
                    }
                }
            }
        }
    }

    fun refreshSubordinates(context: Context) {
        loadSubordinates(context)
    }
}

class SubordinatesViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(SubordinatesViewModel::class.java)) {
            return SubordinatesViewModel(context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}
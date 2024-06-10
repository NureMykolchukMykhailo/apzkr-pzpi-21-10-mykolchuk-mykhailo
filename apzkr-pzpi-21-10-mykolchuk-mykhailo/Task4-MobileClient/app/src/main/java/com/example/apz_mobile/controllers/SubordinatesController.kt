package com.example.apz_mobile.controllers

import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Subordinate
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.storage.TokenManager
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class SubordinatesController(private val apiService: ApiService, private val tokenManager: TokenManager) {

    fun getSubordinatesByChief(onResult: (List<Subordinate>?, Boolean) -> Unit) {
        apiService.getSubordinatesByChief().enqueue(object : Callback<List<Subordinate>> {
            override fun onResponse(call: Call<List<Subordinate>>, response: Response<List<Subordinate>>) {
                if (response.isSuccessful) {
                    onResult(response.body(), true)
                } else {
                    if (response.code() == 401) {
                        tokenManager.clearToken()
                        onResult(null, false)
                    } else {
                        onResult(null, true)
                    }
                }
            }

            override fun onFailure(call: Call<List<Subordinate>>, t: Throwable) {
                onResult(null, true)
            }
        })
    }

    fun registerSubordinate(name: String, email: String, password: String, onResult: (Boolean) -> Unit) {
        apiService.registerSubordinate(name, email, password).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }

    fun getCarsByOwner(onResult: (List<Car>?) -> Unit) {
        apiService.getCarsByOwner().enqueue(object : Callback<List<Car>> {
            override fun onResponse(call: Call<List<Car>>, response: Response<List<Car>>) {
                onResult(response.body())
            }

            override fun onFailure(call: Call<List<Car>>, t: Throwable) {
                onResult(null)
            }
        })
    }

    fun connectSubordinateToCar(subordinateId: Int, carId: Int, onResult: (Boolean) -> Unit) {
        apiService.subordinateConnectToCar(subordinateId, carId).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }

    fun disconnectSubordinateFromCar(subordinateId: Int, onResult: (Boolean) -> Unit) {
        apiService.subordinateDisconnectFromCar(subordinateId).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }
}

package com.example.apz_mobile.controllers

import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Trip
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.storage.TokenManager
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class CarsController(private val apiService: ApiService, private val tokenManager: TokenManager) {

    fun getCarsByOwner(onResult: (List<Car>?, Boolean) -> Unit) {
        apiService.getCarsByOwner().enqueue(object : Callback<List<Car>> {
            override fun onResponse(call: Call<List<Car>>, response: Response<List<Car>>) {
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

            override fun onFailure(call: Call<List<Car>>, t: Throwable) {
                onResult(null, true)
            }
        })
    }
    fun addCar(name: String, type: String, onResult: (Boolean) -> Unit) {
        apiService.addCar(name, type).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }

    fun getCarTrips(carId: Int, onResult: (List<Trip>?, Boolean) -> Unit) {
        apiService.getCarTrips(carId).enqueue(object : Callback<List<Trip>> {
            override fun onResponse(call: Call<List<Trip>>, response: Response<List<Trip>>) {
                if (response.isSuccessful) {
                    onResult(response.body(), true)
                } else {
                    onResult(null, true)
                }
            }

            override fun onFailure(call: Call<List<Trip>>, t: Throwable) {
                onResult(null, true)
            }
        })
    }
}

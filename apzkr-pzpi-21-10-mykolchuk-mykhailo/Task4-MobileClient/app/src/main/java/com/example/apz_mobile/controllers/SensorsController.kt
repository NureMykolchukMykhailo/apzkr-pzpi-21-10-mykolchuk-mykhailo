package com.example.apz_mobile.controllers

import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Sensor
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.storage.TokenManager
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class SensorsController(private val apiService: ApiService, private val tokenManager: TokenManager) {

    fun getSensorsByOwner(onResult: (List<Sensor>?, Boolean) -> Unit) {
        apiService.getSensorsByOwner().enqueue(object : Callback<List<Sensor>> {
            override fun onResponse(call: Call<List<Sensor>>, response: Response<List<Sensor>>) {
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

            override fun onFailure(call: Call<List<Sensor>>, t: Throwable) {
                onResult(null, true)
            }
        })
    }

    fun addSensor(name: String, onResult: (Boolean) -> Unit) {
        apiService.addSensor(name).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }

    fun getCarsWithoutSensors(onResult: (List<Car>?) -> Unit) {
        apiService.getCarsWithoutSensors().enqueue(object : Callback<List<Car>> {
            override fun onResponse(call: Call<List<Car>>, response: Response<List<Car>>) {
                onResult(response.body())
            }

            override fun onFailure(call: Call<List<Car>>, t: Throwable) {
                onResult(null)
            }
        })
    }

    fun connectSensorToCar(sensorId: Int, carId: Int, onResult: (Boolean) -> Unit) {
        apiService.sensorConnectToCar(sensorId, carId).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }

    fun disconnectSensorFromCar(carId: Int, onResult: (Boolean) -> Unit) {
        apiService.sensorDisconnectFromCar(carId).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                onResult(response.isSuccessful)
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                onResult(false)
            }
        })
    }
}

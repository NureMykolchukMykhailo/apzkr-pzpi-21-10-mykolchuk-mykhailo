package com.example.apz_mobile.network

import com.example.apz_mobile.models.Car
import com.example.apz_mobile.models.Sensor
import com.example.apz_mobile.models.Subordinate
import com.example.apz_mobile.models.Trip
import com.example.apz_mobile.models.UserDetail
import okhttp3.RequestBody
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part
import retrofit2.http.Query


data class LoginResponse(val token: String, val email: String, val lang: String)

interface ApiService {
    @Multipart
    @POST("api/authorizeUser")
    fun login(
        @Part("email") username: RequestBody,
        @Part("password") password: RequestBody
    ): Call<LoginResponse>

    @GET("Car/byOwner")
    fun getCarsByOwner(): Call<List<Car>>

    @GET("Subordinate/byChief")
    fun getSubordinatesByChief(): Call<List<Subordinate>>

    @POST("Car/insert")
    fun addCar(@Query("carName") name: String, @Query("type") type: String): Call<Void>

    @GET("Record/byCar")
    fun getCarTrips(@Query("carId") carId: Int): Call<List<Trip>>

    @GET("Sensor/byOwner")
    fun getSensorsByOwner(): Call<List<Sensor>>

    @POST("Sensor/insert")
    fun addSensor(@Query("name") name: String): Call<Void>

    @GET("Car/withoutSensors")
    fun getCarsWithoutSensors(): Call<List<Car>>

    @POST("Sensor/connectToCar")
    fun sensorConnectToCar(@Query("sensorId") sensorId: Int, @Query("carId") carId: Int): Call<Void>

    @POST("Sensor/disconnectFromCar")
    fun sensorDisconnectFromCar(@Query("carId") carId: Int): Call<Void>

    @POST("Subordinate/connectToCar")
    fun subordinateConnectToCar(@Query("subordinateId") subordinateId: Int, @Query("carId") carId: Int): Call<Void>

    @POST("Subordinate/disconnectFromCar")
    fun subordinateDisconnectFromCar(@Query("subordinateId") subordinateId: Int): Call<Void>

    @GET("User/user")
    fun getUserDetails(): Call<UserDetail>

    @POST("api/registerSubordinate")
    fun registerSubordinate(@Query("name") name: String, @Query("email") email: String,
                            @Query("password") password: String): Call<Void>

    @POST("api/registerUser")
    fun registerUser(@Query("name") name: String, @Query("email") email: String,
                            @Query("password") password: String, @Query("type") type: String):  Call<LoginResponse>

}


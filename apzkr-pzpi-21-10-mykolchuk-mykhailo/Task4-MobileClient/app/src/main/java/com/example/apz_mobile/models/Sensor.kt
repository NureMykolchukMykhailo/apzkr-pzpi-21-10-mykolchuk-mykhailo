package com.example.apz_mobile.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize

@Parcelize
data class Sensor (
    val id: Int,
    val name: String,
    val car: CarForSensor?
)  : Parcelable

@Parcelize
data class CarForSensor(
    val id: Int,
    val name: String,
    val type: String,
    val added: String,
    val drivers: List<Driver> = listOf(),
)  : Parcelable
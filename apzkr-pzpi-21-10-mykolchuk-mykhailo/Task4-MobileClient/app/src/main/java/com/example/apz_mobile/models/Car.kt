package com.example.apz_mobile.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize

@Parcelize
data class Car(
    val id: Int,
    val name: String,
    val ownerId: Int,
    val type: String,
    val added: String,
    val sensor: SensorForCar?,
    val drivers: List<Driver> = listOf()
) : Parcelable
{
    override fun toString(): String {
        return "$name ($type)"
    }
}

@Parcelize
data class SensorForCar(val id: Int, val name: String) : Parcelable

@Parcelize
data class Driver(val id: Int, val name: String, val email: String, val regDate: String) :
    Parcelable
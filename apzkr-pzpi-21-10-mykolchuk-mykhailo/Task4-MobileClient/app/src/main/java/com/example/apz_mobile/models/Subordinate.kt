package com.example.apz_mobile.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize

@Parcelize
data class Subordinate(
    val id: Int,
    val chiefId: Int,
    val car: CarForSubordinate?,
    val name: String,
    val email: String,
    val regDate: String,
    val language: String
) : Parcelable

@Parcelize
data class CarForSubordinate(
    val id: Int,
    val name: String,
    val type: String,
    val added: String,
    val sensor: SensorForCar?
) : Parcelable
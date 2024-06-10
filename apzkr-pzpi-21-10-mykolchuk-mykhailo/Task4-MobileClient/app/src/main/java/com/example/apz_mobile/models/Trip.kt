package com.example.apz_mobile.models

data class Trip(
    val id: Int,
    val tripStart: String,
    val tripEnd: String,
    val carId: Int,
    val fastStart: Int,
    val leftTurns: Int,
    val rightTurns: Int,
    val dangerousLeftTurns: Int,
    val dangerousRightTurns: Int,
    val engineSpeeds: List<EngineSpeed> = listOf(),
    val suddenBraking: List<SuddenBraking> = listOf()
)

data class EngineSpeed(
    val id: Int,
    val recordId: Int,
    val begin: String,
    val end: String,
    val avgEngineSpeed: Double
)

data class SuddenBraking(
    val id: Int,
    val recordId: Int,
    val time: String,
    val initialSpeed: Double,
    val subsequentSpeed: Double
)

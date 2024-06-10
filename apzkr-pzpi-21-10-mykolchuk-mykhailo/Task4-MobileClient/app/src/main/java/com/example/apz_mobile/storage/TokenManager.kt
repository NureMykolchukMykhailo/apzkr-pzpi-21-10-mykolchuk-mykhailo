package com.example.apz_mobile.storage

import android.content.Context
import android.content.SharedPreferences

class TokenManager(context: Context) {

    private val prefs: SharedPreferences = context.getSharedPreferences("prefs", Context.MODE_PRIVATE)

    fun saveToken(token: String) {
        prefs.edit().putString("TOKEN_KEY", token).apply()
    }

    fun getToken(): String? {
        return prefs.getString("TOKEN_KEY", null)
    }

    fun clearToken() {
        prefs.edit().remove("TOKEN_KEY").apply()
    }
}


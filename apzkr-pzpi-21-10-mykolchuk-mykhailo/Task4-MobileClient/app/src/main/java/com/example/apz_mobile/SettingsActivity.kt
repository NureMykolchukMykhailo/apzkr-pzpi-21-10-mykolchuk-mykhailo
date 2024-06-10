package com.example.apz_mobile

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.apz_mobile.storage.LocaleHelper

class SettingsActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_settings)

        val buttonEnglish = findViewById<Button>(R.id.button_english)
        val buttonUkrainian = findViewById<Button>(R.id.button_ukrainian)

        buttonEnglish.setOnClickListener {
            setLocale("en")
        }

        buttonUkrainian.setOnClickListener {
            setLocale("ua")
        }
    }

    private fun setLocale(language: String) {
        val prefs = getSharedPreferences("prefs", Context.MODE_PRIVATE)
        prefs.edit().putString("USER_LANG", language).apply()
        LocaleHelper.setLocale(this, language)

        val intent = Intent("LANGUAGE_CHANGED")
        LocalBroadcastManager.getInstance(this).sendBroadcast(intent)

        recreate()
        finish()
    }
}

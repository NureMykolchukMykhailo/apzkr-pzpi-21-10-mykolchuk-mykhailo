package com.example.apz_mobile

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.TextView
import android.widget.Toast
import com.google.android.material.navigation.NavigationView
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import androidx.drawerlayout.widget.DrawerLayout
import androidx.appcompat.app.AppCompatActivity
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.apz_mobile.databinding.ActivityMainBinding
import com.example.apz_mobile.models.UserDetail
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.LocaleHelper
import com.example.apz_mobile.storage.TokenManager
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding
    private lateinit var tokenManager: TokenManager

    private val languageChangeReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context?, intent: Intent?) {
            recreate()
        }
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val prefs = getSharedPreferences("prefs", Context.MODE_PRIVATE)
        val language = prefs.getString("USER_LANG", "en")
        if (language != null) {
            LocaleHelper.setLocale(this, language)
        }

        tokenManager = TokenManager(this)
        if (tokenManager.getToken() == null) {
            startActivity(Intent(this, LoginActivity::class.java))
            finish()
            return
        }
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.appBarMain.toolbar)

        setupFabButton()

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)

        appBarConfiguration = AppBarConfiguration(setOf(
            R.id.nav_subordinates, R.id.nav_sensors, R.id.nav_cars), drawerLayout)

        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        navController.addOnDestinationChangedListener { _, destination, _ ->
            updateFabButton(destination.id)
        }

        fetchUserDetails { userDetail ->
            if (userDetail != null) {
                updateNavHeader(userDetail.name, userDetail.email, userDetail.regDate)
            } else {
                Toast.makeText(this@MainActivity, "UserDetail is null", Toast.LENGTH_SHORT).show()
            }
        }
        LocalBroadcastManager.getInstance(this).registerReceiver(languageChangeReceiver, IntentFilter("LANGUAGE_CHANGED"))


    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.action_settings -> {
                val intent = Intent(this, SettingsActivity::class.java)
                startActivity(intent)
                true
            }
            R.id.action_exit -> {
                logout()
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }
    private fun logout() {
        tokenManager.clearToken()

        val intent = Intent(this, LoginActivity::class.java)
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK)
        startActivity(intent)
        finish()
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }
    override fun onDestroy() {
        super.onDestroy()
        LocalBroadcastManager.getInstance(this).unregisterReceiver(languageChangeReceiver)
    }

    private fun setupFabButton() {
        binding.appBarMain.fab.setOnClickListener {
            val navController = findNavController(R.id.nav_host_fragment_content_main)
            val currentFragment = navController.currentDestination?.id
            if (currentFragment != null && isAddableFragment(currentFragment)) {
                (supportFragmentManager.findFragmentById(R.id.nav_host_fragment_content_main)
                    ?.childFragmentManager?.fragments?.get(0) as? AddableFragment)?.onAddButtonClicked()
            } else {
                Toast.makeText(this, "Add action not supported", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun updateFabButton(fragmentId: Int) {
        if (isAddableFragment(fragmentId)) {
            binding.appBarMain.fab.show()
        } else {
            binding.appBarMain.fab.hide()
        }
    }

    private fun isAddableFragment(fragmentId: Int): Boolean {
        return when (fragmentId) {
            R.id.nav_sensors, R.id.nav_cars, R.id.nav_subordinates -> true
            else -> false
        }
    }

    private fun updateNavHeader(name: String?, email: String?, regDate: String?) {
        val navView: NavigationView = binding.navView
        val headerView = navView.getHeaderView(0)
        val navUsername = headerView.findViewById<TextView>(R.id.nav_header_username)
        val navEmail = headerView.findViewById<TextView>(R.id.nav_header_email)
        val navRegDate = headerView.findViewById<TextView>(R.id.nav_header_regdate)
        navUsername.text = name
        navEmail.text = email
        navRegDate.text = regDate
    }

    private fun fetchUserDetails(callback: (UserDetail?) -> Unit) {
        val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
        apiService.getUserDetails().enqueue(object : Callback<UserDetail> {
            override fun onResponse(call: Call<UserDetail>, response: Response<UserDetail>) {
                if (response.isSuccessful && response.body() != null) {
                    callback(response.body())
                } else {
                    Toast.makeText(this@MainActivity, "Failed to fetch user details", Toast.LENGTH_SHORT).show()
                    callback(null)
                }
            }

            override fun onFailure(call: Call<UserDetail>, t: Throwable) {
                Toast.makeText(this@MainActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
                callback(null)
            }
        })
    }
}
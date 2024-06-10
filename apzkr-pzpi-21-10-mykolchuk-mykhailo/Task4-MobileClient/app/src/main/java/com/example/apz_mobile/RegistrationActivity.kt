package com.example.apz_mobile

import android.content.Intent
import android.os.Bundle
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.apz_mobile.models.UserDetail
import com.example.apz_mobile.network.ApiService
import com.example.apz_mobile.network.LoginResponse
import com.example.apz_mobile.network.RetrofitClient
import com.example.apz_mobile.storage.TokenManager
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.RequestBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RegistrationActivity : AppCompatActivity(){
    private lateinit var userNameEditText: EditText
    private lateinit var userEmailEditText: EditText
    private lateinit var userPasswordEditText: EditText
    private lateinit var userTypeSpinner: Spinner
    private lateinit var registerButton: Button
    private lateinit var loginButton: Button
    private lateinit var tokenManager: TokenManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_registration)

        userNameEditText = findViewById(R.id.user_name)
        userEmailEditText = findViewById(R.id.user_email)
        userPasswordEditText = findViewById(R.id.user_password)
        userTypeSpinner = findViewById(R.id.user_type_spinner)
        registerButton = findViewById(R.id.reg_button)
        loginButton = findViewById(R.id.login_button)

        tokenManager =  TokenManager(this)

        ArrayAdapter.createFromResource(
            this,
            R.array.user_types,
            android.R.layout.simple_spinner_item
        ).also { adapter ->
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            userTypeSpinner.adapter = adapter
        }

        registerButton.setOnClickListener {
            val name = userNameEditText.text.toString()
            val email = userEmailEditText.text.toString()
            val password = userPasswordEditText.text.toString()
            val userType = userTypeSpinner.selectedItem.toString()

            registerUser(name, email, password, userType)
        }

        loginButton.setOnClickListener {
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
            finish()
        }
    }

    private fun registerUser(name: String, email: String, password: String, userType: String) {
        val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)

        val call = apiService.registerUser(name, email, password, userType)
        call.enqueue(object : Callback<LoginResponse> {
            override fun onResponse(call: Call<LoginResponse>, response: Response<LoginResponse>) {
                if (response.isSuccessful && response.body() != null) {
                    val token = response.body()!!.token
                    tokenManager.saveToken(token)
                    fetchUserDetails()
                } else {
                    Toast.makeText(this@RegistrationActivity, "Login failed", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                Toast.makeText(this@RegistrationActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    fun fetchUserDetails() {
        val apiService = RetrofitClient.getInstance(tokenManager).create(ApiService::class.java)
        apiService.getUserDetails().enqueue(object : Callback<UserDetail> {
            override fun onResponse(call: Call<UserDetail>, response: Response<UserDetail>) {
                if (response.isSuccessful && response.body() != null) {
                    val userDetails = response.body()!!
                    val intent = Intent(this@RegistrationActivity, MainActivity::class.java).apply {
                        putExtra("USER_NAME", userDetails.name)
                        putExtra("USER_EMAIL", userDetails.email)
                        putExtra("USER_LANG", userDetails.language)
                        putExtra("USER_REGDATE", userDetails.regDate)
                    }
                    startActivity(intent)
                    finish()
                } else {
                    Toast.makeText(this@RegistrationActivity, "Failed to fetch user details", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<UserDetail>, t: Throwable) {
                Toast.makeText(this@RegistrationActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
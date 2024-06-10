package com.example.apz_mobile.ui.cars

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.Driver


class DriverAdapter(private val drivers: List<Driver>) : RecyclerView.Adapter<DriverAdapter.DriverViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DriverViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_driver, parent, false)
        return DriverViewHolder(view)
    }

    override fun onBindViewHolder(holder: DriverViewHolder, position: Int) {
        holder.bind(drivers[position])
    }

    override fun getItemCount(): Int = drivers.size

    class DriverViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val driverName: TextView = itemView.findViewById(R.id.driver_name)
        private val driverEmail: TextView = itemView.findViewById(R.id.driver_email)
        private val driverRegistrationDate: TextView = itemView.findViewById(R.id.driver_registration_date)

        fun bind(driver: Driver) {
            driverName.text = driver.name
            driverEmail.text = driver.email
            driverRegistrationDate.text = driver.regDate
        }
    }
}

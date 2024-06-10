package com.example.apz_mobile.ui.cars

import android.content.Context
import android.os.Build
import android.provider.Settings.Global.getString
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.Car
import java.time.LocalDateTime

class CarsAdapter(
    private val context: Context,
    private val cars: List<Car>,
    private val onCarClicked: (Car) -> Unit
) : RecyclerView.Adapter<CarsAdapter.CarViewHolder>() {

    class CarViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val carName: TextView = view.findViewById(R.id.car_name_type)
        val carType: TextView = view.findViewById(R.id.car_type)
        val carAdded: TextView = view.findViewById(R.id.car_added)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CarViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_car, parent, false)
        return CarViewHolder(view)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: CarViewHolder, position: Int) {
        val car = cars[position]
        holder.carName.text = car.name
        holder.carType.text = car.type
        holder.carAdded.text =
            context.getString(R.string.car_fragment_car_added) + LocalDateTime.parse(car.added).toString()

        holder.itemView.setOnClickListener {
            onCarClicked(car)
        }
    }

    override fun getItemCount() = cars.size
}


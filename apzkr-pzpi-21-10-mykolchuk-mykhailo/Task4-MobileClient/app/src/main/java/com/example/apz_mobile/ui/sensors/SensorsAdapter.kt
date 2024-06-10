package com.example.apz_mobile.ui.sensors

import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.Sensor

class SensorsAdapter(
    private val sensors: List<Sensor>,
    private val onSensorClicked: (Sensor) -> Unit
) : RecyclerView.Adapter<SensorsAdapter.SensorViewHolder>() {

    class SensorViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val sensorName: TextView = view.findViewById(R.id.sensor_name)
        val connectedCar: TextView = view.findViewById(R.id.connected_car)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SensorViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_sensor, parent, false)
        return SensorViewHolder(view)
    }

    override fun onBindViewHolder(holder: SensorViewHolder, position: Int) {
        val sensor = sensors[position]
        holder.sensorName.text = sensor.name
        if(sensor.car != null){
            holder.connectedCar.text = sensor.car.name + " (" + sensor.car.type + ")"
        }
        else{
            holder.connectedCar.text = "Not connected to car"
            holder.connectedCar.setTextColor(Color.RED)
        }

        holder.itemView.setOnClickListener {
            onSensorClicked(sensor)
        }
    }

    override fun getItemCount() = sensors.size
}
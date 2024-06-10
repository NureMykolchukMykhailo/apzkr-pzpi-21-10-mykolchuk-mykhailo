package com.example.apz_mobile.ui.trips

import android.content.Context
import android.os.Build
import android.support.annotation.RequiresApi
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.EngineSpeed
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import kotlin.math.roundToInt

class EngineSpeedAdapter(
    private val context: Context,
    private val engineSpeeds: List<EngineSpeed>) : RecyclerView.Adapter<EngineSpeedAdapter.EngineSpeedViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): EngineSpeedViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_engine_speed, parent, false)
        return EngineSpeedViewHolder(view)
    }

    @androidx.annotation.RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: EngineSpeedViewHolder, position: Int) {
        val engineSpeed = engineSpeeds[position]
        holder.bind(context, engineSpeed)
    }

    override fun getItemCount() = engineSpeeds.size

    class EngineSpeedViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val avgEngineSpeed: TextView = itemView.findViewById(R.id.avg_engine_speed)
        private val momentBegin: TextView = itemView.findViewById(R.id.moment_begin)
        private val momentEnd: TextView = itemView.findViewById(R.id.moment_end)

        @androidx.annotation.RequiresApi(Build.VERSION_CODES.O)
        fun bind(context: Context,engineSpeed: EngineSpeed) {
            avgEngineSpeed.text = context.getString(R.string.avg_engine_speed) + engineSpeed.avgEngineSpeed.roundToInt().toString()
            momentBegin.text = context.getString(R.string.engine_moment_start) + formatDateTime(engineSpeed.begin)
            momentEnd.text = context.getString(R.string.engine_moment_end) + formatDateTime(engineSpeed.end)
        }

        @RequiresApi(Build.VERSION_CODES.O)
        private fun formatDateTime(dateTimeString: String): String {
            val inputFormatter = DateTimeFormatter.ofPattern("dd.MM.yyyy HH:mm:ss")
            val outputFormatter = DateTimeFormatter.ofPattern("HH:mm:ss")

            val dateTime = LocalDateTime.parse(dateTimeString, inputFormatter)
            return dateTime.format(outputFormatter)
        }
    }

}

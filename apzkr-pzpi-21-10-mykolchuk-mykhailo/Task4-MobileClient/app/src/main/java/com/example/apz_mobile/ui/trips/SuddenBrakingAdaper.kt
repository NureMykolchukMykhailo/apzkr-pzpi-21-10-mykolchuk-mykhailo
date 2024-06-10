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
import com.example.apz_mobile.models.SuddenBraking
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class SuddenBrakingAdapter(
    private val context: Context,
    private val suddenBraking: List<SuddenBraking>) : RecyclerView.Adapter<SuddenBrakingAdapter.SuddenBrakingViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SuddenBrakingViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_sudden_braking, parent, false)
        return SuddenBrakingViewHolder(view)
    }

    @androidx.annotation.RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: SuddenBrakingViewHolder, position: Int) {
        val braking = suddenBraking[position]
        holder.bind(context,braking)
    }

    override fun getItemCount() = suddenBraking.size

    class SuddenBrakingViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val initialSpeed: TextView = itemView.findViewById(R.id.initial_speed)
        private val subsequentSpeed: TextView = itemView.findViewById(R.id.subsequent_speed)
        private val time: TextView = itemView.findViewById(R.id.moment_time)

        @androidx.annotation.RequiresApi(Build.VERSION_CODES.O)
        fun bind(context: Context, braking: SuddenBraking) {
            initialSpeed.text = context.getString(R.string.sudden_braking_start_speed) + braking.initialSpeed.toString()
            subsequentSpeed.text = context.getString(R.string.sudden_braking_end_speed) + braking.subsequentSpeed.toString()
            time.text = context.getString(R.string.sudden_braking_time) + formatDateTime(braking.time)
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

package com.example.apz_mobile.ui.trips


import android.content.Context
import android.graphics.Color
import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.Trip

class TripAdapter(
    private val context: Context,
    private val trips: List<Trip>) : RecyclerView.Adapter<TripAdapter.TripViewHolder>() {

    class TripViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val tripStart: TextView = view.findViewById(R.id.trip_start)
        val tripEnd: TextView = view.findViewById(R.id.trip_end)
        val fastStart: TextView = view.findViewById(R.id.fast_start)
        val leftTurns: TextView = view.findViewById(R.id.left_turns)
        val rightTurns: TextView = view.findViewById(R.id.right_turns)
        val dangerousLeftTurns: TextView = view.findViewById(R.id.dangerous_left_turns)
        val dangerousRightTurns: TextView = view.findViewById(R.id.dangerous_right_turns)
        val btnToggleEngineSpeeds: Button = view.findViewById(R.id.btn_toggle_engine_speeds)
        val recyclerEngineSpeeds: RecyclerView = view.findViewById(R.id.recycler_engine_speeds)
        val btnToggleSuddenBraking: Button = view.findViewById(R.id.btn_toggle_sudden_braking)
        val recyclerSuddenBraking: RecyclerView = view.findViewById(R.id.recycler_sudden_braking)

        fun bind(context: Context, trip: Trip) {

            recyclerEngineSpeeds.apply {
                layoutManager = LinearLayoutManager(itemView.context, LinearLayoutManager.HORIZONTAL, false)
                adapter = EngineSpeedAdapter(context, trip.engineSpeeds)
            }

            recyclerSuddenBraking.apply {
                layoutManager = LinearLayoutManager(itemView.context, LinearLayoutManager.HORIZONTAL, false)
                adapter = SuddenBrakingAdapter(context,trip.suddenBraking)
            }

            btnToggleEngineSpeeds.setOnClickListener {
                if (recyclerEngineSpeeds.visibility == View.GONE) {
                    recyclerEngineSpeeds.visibility = View.VISIBLE
                    btnToggleEngineSpeeds.text = context.getString(R.string.hide_engine_speeds)
                } else {
                    recyclerEngineSpeeds.visibility = View.GONE
                    btnToggleEngineSpeeds.text = context.getString(R.string.show_engine_speeds)
                }
            }

            btnToggleSuddenBraking.setOnClickListener {
                if (recyclerSuddenBraking.visibility == View.GONE) {
                    recyclerSuddenBraking.visibility = View.VISIBLE
                    btnToggleSuddenBraking.text = context.getString(R.string.hide_sudden_braking)
                } else {
                    recyclerSuddenBraking.visibility = View.GONE
                    btnToggleSuddenBraking.text = context.getString(R.string.show_sudden_braking)
                }
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): TripViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.trip_item, parent, false)
        return TripViewHolder(view)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: TripViewHolder, position: Int) {
        val trip = trips[position]

        holder.tripStart.text = context.getString(R.string.car_detail_trip_start) + trip.tripStart
        holder.tripEnd.text = context.getString(R.string.car_detail_trip_end) + trip.tripEnd

        if(trip.fastStart == 0){
            holder.fastStart.text =  context.getString(R.string.car_detail_normal_start)
            holder.fastStart.setTextColor(Color.parseColor("#32CD32"))
        } else if(trip.fastStart > 0){
            holder.fastStart.text =  context.getString(R.string.car_detail_fast_start_front)
            holder.fastStart.setTextColor(Color.parseColor("#FF0000"))
        }
        else{
            holder.fastStart.text = context.getString(R.string.car_detail_fast_start_back)
            holder.fastStart.setTextColor(Color.parseColor("#FF0000"))
        }
        holder.leftTurns.text =  context.getString(R.string.car_detail_left_turns) + trip.leftTurns.toString()
        holder.rightTurns.text = context.getString(R.string.car_detail_right_turns) + trip.rightTurns.toString()
        holder.dangerousLeftTurns.text = context.getString(R.string.car_detail_deng_left_turns) + trip.dangerousLeftTurns.toString()
        holder.dangerousRightTurns.text = context.getString(R.string.car_detail_deng_right_turns) + trip.dangerousRightTurns.toString()

        holder.bind(context, trip)

    }

    override fun getItemCount() = trips.size
}

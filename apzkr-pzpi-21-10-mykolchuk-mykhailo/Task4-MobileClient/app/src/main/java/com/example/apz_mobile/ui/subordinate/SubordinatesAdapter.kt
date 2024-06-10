package com.example.apz_mobile.ui.subordinate

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.apz_mobile.R
import com.example.apz_mobile.models.Subordinate

class SubordinatesAdapter(
    private val subordinates: List<Subordinate>,
    private val onSubordinateClicked: (Subordinate) -> Unit
) : RecyclerView.Adapter<SubordinatesAdapter.SubordinateViewHolder>() {

    class SubordinateViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val subordinateName: TextView = view.findViewById(R.id.subordinate_Name)
        val subordinateEmail: TextView = view.findViewById(R.id.subordinate_Email)
        val subordinateRegDate: TextView = view.findViewById(R.id.subordinate_RegDate)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SubordinateViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_subordinate, parent, false)
        return SubordinateViewHolder(view)
    }

    override fun onBindViewHolder(holder: SubordinateViewHolder, position: Int) {
        val subordinate = subordinates[position]
        holder.subordinateName.text = subordinate.name
        holder.subordinateEmail.text = subordinate.email
        holder.subordinateRegDate.text = subordinate.regDate

        holder.itemView.setOnClickListener {
            onSubordinateClicked(subordinate)
        }
    }

    override fun getItemCount() = subordinates.size
}
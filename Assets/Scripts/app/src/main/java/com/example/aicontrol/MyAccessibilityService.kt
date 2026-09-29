package com.example.aicontrol

import android.accessibilityservice.AccessibilityService
import android.accessibilityservice.GestureDescription
import android.graphics.Path
import android.view.accessibility.AccessibilityEvent
import org.json.JSONObject

class MyAccessibilityService : AccessibilityService() {

    companion object {
        var instance: MyAccessibilityService? = null
            private set
    }

    override fun onServiceConnected() {
        super.onServiceConnected()
        instance = this
    }

    override fun onAccessibilityEvent(event: AccessibilityEvent?) {}
    override fun onInterrupt() {}

    fun handleJsonCommand(jsonStr: String): String {
        return try {
            val json = JSONObject(jsonStr)
            when (json.optString("action")) {
                "click" -> {
                    performGlobalAction(GLOBAL_ACTION_BACK)
                    "Clicked"
                }
                "scroll" -> {
                    "Scrolled"
                }
                else -> "Unknown action"
            }
        } catch (e: Exception) {
            "Error: ${e.message}"
        }
    }
}

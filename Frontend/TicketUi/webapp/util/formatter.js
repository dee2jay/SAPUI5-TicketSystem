sap.ui.define([
    "sap/ui/core/format/DateFormat"
], function (DateFormat) {
    "use strict";

    /**
     * Utility: Normalize date string (fix Safari issues)
     */
    function normalizeDate(value) {
        if (!value) return null;

        // Fix strings like "2025-02-21 15:20:00" → "2025-02-21T15:20:00"
        if (typeof value === "string" && value.indexOf(" ") > -1 && value.indexOf("T") === -1) {
            value = value.replace(" ", "T");
        }

        const d = new Date(value);
        if (isNaN(d.getTime())) return null;

        return d;
    }

    /**
     * Utility: get locale from UI5 or browser
     */
    function getLocale(locale) {
        return locale || sap.ui.getCore().getConfiguration().getLocale().toString() || navigator.language;
    }

    /**
     * Utility: remove comma between date + time for DE and some locales
     */
    function removeComma(value) {
        return value.replace(",", "").trim();
    }

    return {

        /**
         * Format date + time (pattern, UI5 locale aware)
         */
        formatDateTime: function (value, locale) {
            const oDate = normalizeDate(value);
            if (!oDate) return "";

            const sLocale = getLocale(locale);

            return DateFormat.getDateTimeInstance({
                pattern: "dd.MM.yyyy HH:mm:ss",
                calendarType: sap.ui.core.CalendarType.Gregorian
            }, new sap.ui.core.Locale(sLocale)).format(oDate);
        },

        /**
         * Format date only
         */
        formatDate: function (value, locale) {
            const oDate = normalizeDate(value);
            if (!oDate) return "";

            const sLocale = getLocale(locale);

            return DateFormat.getDateInstance({
                pattern: "dd.MM.yyyy",
                calendarType: sap.ui.core.CalendarType.Gregorian
            }, new sap.ui.core.Locale(sLocale)).format(oDate);
        },

        /**
         * Format date + time with timezone (Intl-based)
         */
        formatDateTimeWithTimezone: function (value, timezone, locale) {
            const oDate = normalizeDate(value);
            if (!oDate) return "";

            const sLocale = getLocale(locale);
            const sTimezone = timezone || Intl.DateTimeFormat().resolvedOptions().timeZone;

            let options = {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit",
                hour12: false,
                timeZone: sTimezone
            };

            const formatted = new Intl.DateTimeFormat(sLocale, options).format(oDate);
            return removeComma(formatted);
        },

        /**
         * Format date only with timezone
         */
        formatDateWithTimezone: function (value, timezone, locale) {
            const oDate = normalizeDate(value);
            if (!oDate) return "";

            const sLocale = getLocale(locale);
            const sTimezone = timezone || Intl.DateTimeFormat().resolvedOptions().timeZone;

            let options = {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                timeZone: sTimezone
            };

            const formatted = new Intl.DateTimeFormat(sLocale, options).format(oDate);
            return removeComma(formatted);
        },

        /**
         * Return browser timezone
         */
        getBrowserTimezone: function () {
            return Intl.DateTimeFormat().resolvedOptions().timeZone;
        }
    };
});

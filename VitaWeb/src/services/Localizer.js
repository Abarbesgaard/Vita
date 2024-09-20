import moment from "moment";
import { momentLocalizer } from "react-big-calendar";

moment.locale("da-DK", {
	months: [
		"Januar",
		"Februar",
		"Marts",
		"April",
		"Maj",
		"Juni",
		"Juli",
		"August",
		"September",
		"Oktober",
		"November",
		"December",
	],
	monthsShort: [
		"Jan",
		"Feb",
		"Mar",
		"Apr",
		"Maj",
		"Jun",
		"Jul",
		"Aug",
		"Sep",
		"Okt",
		"Nov",
		"Dec",
	],
	weekdays: [
		"Søndag",
		"Mandag",
		"Tirsdag",
		"Onsdag",
		"Torsdag",
		"Fredag",
		"Lørdag",
	],
	weekdaysShort: ["Søn", "Man", "Tir", "Ons", "Tor", "Fre", "Lør"],
	week: { dow: 1 },
	culture: "da-DK",
	day: "Dag",
});

const localizer = momentLocalizer(moment);

localizer.formats.timeGutterFormat = "HH:mm";
localizer.formats.agendaTimeFormat = "HH:mm";
localizer.formats.eventTimeRangeFormat = ({ start, end }, culture, local) => {
	const s = local.format(start, "HH:mm", culture);
	const e = local.format(end, "HH:mm", culture);
	return `${s} - ${e}`;
};
localizer.formats.agendaTimeRangeFormat = ({ start, end }, culture, local) => {
	const s = local.format(start, "HH:mm", culture);
	const e = local.format(end, "HH:mm", culture);
	return `${s} - ${e}`;
};

export default localizer;

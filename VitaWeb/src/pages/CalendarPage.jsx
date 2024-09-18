import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import Layout from "../components/Layout";

moment.locale("da_DK", {
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
});

const localizer = momentLocalizer(moment);

const CalendarPage = () => {
	return (
		<Layout>
			<div className="h-full w-full bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-no-repeat bg-center">
				<div className="bg-white bg-opacity-80 backdrop-blur">
					<Calendar
						localizer={localizer}
						startAccessor="start"
						endAccessor="end"
						events={[
							{
								title: "My event",
								start: new Date(),
								end: new Date(),
							},
							{
								title: "Team 1 møde",
								start: new Date(2024, 8, 19, 10, 45),
								end: new Date(2024, 8, 19, 11, 45),
							},
							{
								title: "Guildmøde",
								start: new Date(2024, 8, 20),
								end: new Date(2024, 8, 20),
							},
						]}
						style={{ height: "100vh" }}
					/>
				</div>
			</div>
		</Layout>
	);
};

export default CalendarPage;

import { Calendar } from "react-big-calendar";
import { Calendar as SmallCalendar } from "react-calendar";
import localizer from "../services/Localizer";
import "../components/Calendar/CalendarStyle.css";
import "react-calendar/dist/Calendar.css";
import { useState } from "react";
import EventModal from "../components/Calendar/EventModal";
import { AnimatePresence } from "framer-motion";

const messages = {
	date: "Dato",
	time: "Tid",
	event: "Event",
	allDay: "Hele dagen",
	week: "Uge",
	day: "Dag",
	month: "Måned",
	previous: "Forrige",
	next: "Næste",
	yesterday: "Igår",
	tommorow: "I morgen",
	today: "I dag",
	agenda: "Agenda",
	noEventsInRange: "Ingen events i denne periode",
	showMore: (total) => `+${total} flere`,
};

const resources = [
	{
		id: 1,
		title: "Rulle",
	},
	{
		id: 2,
		title: "Cromwell",
	},
	{
		id: 3,
		title: "Conan Lurbakke",
	},
	{
		id: 4,
		title: "Fyrst Walther",
	},
	{
		id: 5,
		title: "Liva",
	},
	{
		id: 6,
		title: "Kongen",
	},
	{
		id: 7,
		title: "Dronningen",
	},
	{
		id: 8,
		title: "Prinsessen",
	},
	{
		id: 9,
		title: "Prinsen",
	},
];

const events = [
	{
		title: "Fedt event",
		start: new Date(),
		end: new Date(),
		allDay: true,
		resourceId: [1, 2, 3, 5, 7, 8],
		description: "Dette er en beskrivelse",
	},
	{
		title: "Team 1 møde",
		start: new Date(2024, 8, 19, 10, 45),
		end: new Date(2024, 8, 19, 15, 0),
		resourceId: [1, 4],
	},
	{
		title: "Guildmøde",
		start: new Date(2024, 8, 20),
		end: new Date(2024, 8, 20),
		resourceId: [2, 3],
	},
];

const CalendarPage = () => {
	const [selectedDay, setSelectedDay] = useState(new Date());
	const [selectedResources, setSelectedResources] = useState(resources);
	const [showEventModal, setShowEventModal] = useState(false);
	const [selectedEvent, setSelectedEvent] = useState(null);

	const handleChangeSelectedDay = (value) => {
		setSelectedDay(value);
	};

	return (
		<div className="bg-white h-dvh p-10 flex">
			<AnimatePresence>
				{showEventModal && (
					<EventModal
						onClose={() => setShowEventModal(false)}
						event={selectedEvent}
						resources={resources}
					/>
				)}
			</AnimatePresence>
			<div className="w-96 bg-white h-full">
				<div className="flex flex-col space-y-2 items-center pr-10">
					<div className="mb-20">
						<SmallCalendar
							className="shadow-md w-min rounded-md"
							onChange={handleChangeSelectedDay}
							value={selectedDay}
							tileClassName="rounded-full"
							prev2Label={null}
							next2Label={null}
						/>
					</div>
					<div className="columns-2">
						{resources.map((resource) => (
							<div key={resource.id} className="flex">
								<label
									className="mr-2 text-nowrap text-ellipsis"
									htmlFor={resource.title}
								>
									{resource.title}
								</label>
								<input
									className="ml-auto"
									type="checkbox"
									id={resource.title}
									name={resource.title}
									value={resource.title}
									checked={selectedResources.includes(resource)}
									onChange={(e) => {
										if (e.target.checked) {
											setSelectedResources(
												[...selectedResources, resource].sort(
													(a, b) => a.id - b.id
												)
											);
										} else {
											setSelectedResources(
												selectedResources.filter(
													(selectedResource) => selectedResource !== resource
												)
											);
										}
									}}
								/>
							</div>
						))}
					</div>
				</div>
			</div>
			<div className="h-full w-full bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-no-repeat bg-center overflow-hidden">
				<Calendar
					localizer={localizer}
					startAccessor="start"
					endAccessor="end"
					messages={messages}
					defaultView="day"
					views={["month", "day"]}
					resources={selectedResources}
					date={selectedDay}
					onNavigate={handleChangeSelectedDay}
					events={events}
					onDoubleClickEvent={(event) => {
						setSelectedEvent(event);
						setShowEventModal(true);
					}}
					min={new Date(1972, 8, 1, 6, 0)}
					className="h-full bg-white bg-opacity-80 backdrop-blur"
				/>
			</div>
		</div>
	);
};

export default CalendarPage;

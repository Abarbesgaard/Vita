import { Calendar } from "react-big-calendar";
import "./components/CalendarStyle.css";
import "react-calendar/dist/Calendar.css";
import localizer from "../../services/localizer.js";
import { useState, useEffect } from "react";
import { AnimatePresence } from "framer-motion";
import { useAuth } from "../../hooks/useAuth.jsx";
import { Navigate } from "react-router-dom";
import { getAllActivities, createActivity } from "../../APIs/calendarAPI.js";
import { getSessionToken } from "../../services/supabase.js";
import { BsCalendarWeek, BsCalendarEvent } from "react-icons/bs";
import { getUsers } from "../../services/supabase";
import EventModal from "./components/EventModal";
import AddEventModal from "./components/AddEventModal";
import "./components/CalendarStyle.css";
import SmallCalendar from "react-calendar";

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

const CalendarPage = () => {
	const { user } = useAuth();
	const [selectedDay, setSelectedDay] = useState(new Date());
	const [showEventModal, setShowEventModal] = useState(false);
	const [showAddEventModal, setShowAddEventModal] = useState(false);
	const [selectedEvent, setSelectedEvent] = useState(null);
	const [events, setEvents] = useState([]);
	const [users, setUsers] = useState([]);
	const [selectedResources, setSelectedResources] = useState([]);
	const [isLoading, setIsLoading] = useState(true);
	const [selectedSlot, setSelectedSlot] = useState(null);

	const fetchActivities = async () => {
		const token = await getSessionToken();
		console.log(token);
		const { activities, error } = await getAllActivities(token);
		if (error) {
			alert("Der skete en fejl da begivenhederne skulle hentes fra databasen");
			console.log(error);
		}
		if (!error) {
			activities.forEach((activity) => {
				setEvents((events) => [
					...events,
					{
						id: activity.id,
						start: new Date(activity.start),
						end: new Date(activity.end),
						hostId: activity.hostId,
						resourceId: activity.attendee.map((attendee) => attendee.id),
						attendee: activity.attendee,
						accepted: activity.verifiedAttendee,
						allDay: activity.allDayEvent,
						title: activity.title,
						cancelled: activity.cancelled,
						description: activity.description,
					},
				]);
			});
		}
		setIsLoading(false);
		return activities;
	};

	const fetchUsers = async () => {
		const { data: users } = await getUsers();
		users.forEach((user) => {
			const newUser = { id: user.id, name: user.full_name };
			setUsers((users) => [...users, newUser]);
			setSelectedResources((users) => [...users, newUser]);
		});
	};

	useEffect(() => {
		if (events.length === 0) {
			fetchActivities();
		}
		if (users.length === 0) {
			fetchUsers();
		}
	}, []);

	const handleChangeSelectedDay = (value) => {
		setSelectedDay(value);
	};

	const handleSelectSlot = (slotInfo) => {
		setSelectedSlot(slotInfo);
		setShowAddEventModal(true);
	};
	const handleSelectEvent = (event) => {
		setSelectedEvent(event);
	};

	const handleAddEvent = (newEvent) => {
		setEvents((prevEvents) => [...prevEvents, newEvent]);
	};

	if (!user) {
		return <Navigate to="/login" replace />;
	}

	if (isLoading) {
		return (
			<div className="h-full w-full flex flex-col items-center justify-center overflow-hidden">
				<BsCalendarWeek className="text-9xl text-gray-400 animate-bounce" />
				<p className="animate-pulse">Åbner kalender...</p>
			</div>
		);
	}

	return (
		<div id="calendar" className="h-full w-full flex bg-primary">
			<AnimatePresence>
				{showEventModal && (
					<EventModal
						onClose={() => setShowEventModal(false)}
						event={selectedEvent}
						resources={resources}
					/>
				)}
				{showAddEventModal && (
					<AddEventModal
						onClose={() => setShowAddEventModal(false)}
						users={users}
						user={user}
						setEvents={handleAddEvent}
						selectedSlot={selectedSlot}
					/>
				)}
			</AnimatePresence>
			<div className="w-60 h-full hidden lg:flex flex-col justify-center mx-2">
				<div className="w-full flex flex-col space-y-2 items-center">
					<div className="mb-20 pl-2 max-w-full">
						<SmallCalendar
							className="shadow max-w-full rounded-sm"
							onChange={handleChangeSelectedDay}
							value={selectedDay}
							tileClassName="rounded-full"
							prev2Label={null}
							next2Label={null}
						/>
					</div>
					<div className="flex flex-col space-y-2"></div>
					<div className="flex flex-col items-center gap-5">
						<button
							className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 shadow-depth_blue flex items-center"
							onClick={() => setShowAddEventModal(true)}
						>
							<BsCalendarEvent className="mr-2" />
							Ny begivenhed
						</button>
					</div>
				</div>
			</div>
			<div className="h-full w-full">
				<Calendar
					localizer={localizer}
					startAccessor="start"
					endAccessor="end"
					messages={messages}
					defaultView="day"
					views={["month", "day"]}
					resources={users}
					resourceTitleAccessor="name"
					date={selectedDay}
					onNavigate={handleChangeSelectedDay}
					onSelectEvent={handleSelectEvent}
					onSelectSlot={handleSelectSlot}
					selectable={true}
					events={events}
					onDoubleClickEvent={(event) => {
						setSelectedEvent(event);
						setShowEventModal(true);
					}}
					min={new Date(1972, 8, 1, 6, 0)}
					className="h-full p-2 "
					eventPropGetter={(event) => {
						if (event.type === "meeting") {
							return {
								style: {
									backgroundColor: "red",
								},
							};
						}
						if (event.cancelled === true) {
							return {
								style: {
									backgroundColor: "gray",
									textDecoration: "line-through",
									opacity: 0.5,
								},
							};
						}
					}}
				/>
			</div>
		</div>
	);
};

export default CalendarPage;

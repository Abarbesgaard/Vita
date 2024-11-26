import { Calendar } from "react-big-calendar";
import { Calendar as SmallCalendar } from "react-calendar";
import localizer from "../services/Localizer";
import "../components/Calendar/CalendarStyle.css";
import "react-calendar/dist/Calendar.css";
import { useState, useEffect } from "react";
import EventModal from "../components/Calendar/EventModal";
import { AnimatePresence } from "framer-motion";
import Layout from "../components/Layout";
import { useAuth } from "../context/AuthContext";
import { Navigate } from "react-router-dom";
import { getAllActivities } from "../API/ActivityAPI";
import { getSessionToken } from "../services/Supabase";
import { BsCalendarWeek, BsCalendarEvent } from "react-icons/bs";
import { getUsers } from "../services/Supabase";
import AddEventModal from "../components/Calendar/AddEventModal";

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

const defaultEvents = [
	{
		type: "activity",
		title: "Fedt event",
		start: new Date(),
		end: new Date(),
		allDay: true,
		resourceId: [1, 2, 3, 5, 7, 8],
		description: "Dette er en beskrivelse",
		invites: [1, 2, 3, 5, 7, 9],
		accepted: [1, 5, 7],
		declined: [2],
		reccuring: "none", // none, daily, weekly, monthly, yearly
	},
	{
		title: "Team 1 møde",
		start: new Date(2024, 8, 19, 10, 45),
		end: new Date(2024, 8, 19, 15, 0),
		resourceId: [1, 4],
		invites: [],
		accepted: [],
		declined: [],
	},
	{
		title: "Guildmøde",
		start: new Date(2024, 8, 20),
		end: new Date(2024, 8, 20),
		resourceId: [2, 3],
		invites: [],
		accepted: [],
		declined: [],
	},
	{
		type: "meeting",
		title: "Møde 1",
		description: "Dette er et møde",
		subject: "Møde om noget",
		resourceId: [1],
		invites: [2],
		accepted: [],
		declined: [],
		start: new Date(2024, 8, 21, 10, 0),
		end: new Date(2024, 8, 21, 12, 0),
		status: "pending", // pending, accepted, declined
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

	const fetchActivities = async () => {
		const token = await getSessionToken();
		const { activities, error } = await getAllActivities(token);
		if (error) {
			alert("Der skete en fejl da begivenhederne skulle hentes fra databasen");
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

	if (!user) {
		return <Navigate to="/login" replace />;
	}

	if (isLoading) {
		return (
			<Layout>
				<div className="w-full h-full flex flex-col items-center justify-center">
					<BsCalendarWeek className="text-9xl text-gray-400 animate-bounce" />
					<p className="animate-pulse">Åbner kalender...</p>
				</div>
			</Layout>
		);
	}

	return (
		<Layout>
			<div className="bg-white h-full w-full p-10 pt-5 flex">
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
							setEvents={setEvents}
						/>
					)}
				</AnimatePresence>
				<div className="bg-white h-full">
					<div className="flex flex-col space-y-2 items-center">
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
						{/* <div className="columns-2">
							{users.map((resource) => (
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
													users.filter(
														(selectedResource) => selectedResource !== resource
													)
												);
											}
										}}
									/>
								</div>
							))}
						</div> */}
					</div>
				</div>
				<div className="h-full w-full pr-5 bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-no-repeat bg-center overflow-hidden">
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
						events={events}
						onDoubleClickEvent={(event) => {
							setSelectedEvent(event);
							setShowEventModal(true);
						}}
						scrollToTime={new Date(1972, 8, 1, 6, 0)}
						className="h-full bg-white bg-opacity-80 backdrop-blur"
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
		</Layout>
	);
};

export default CalendarPage;

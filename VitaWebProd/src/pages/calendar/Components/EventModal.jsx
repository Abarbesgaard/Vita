import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useState } from "react";
import { FiEdit } from "react-icons/fi";
import { PiTrash } from "react-icons/pi";
import { updateActivity, deleteActivity } from "../../../APIs/calendarAPI";
import { getSessionToken } from "../../../services/supabase";
import { PiSpinnerGap } from "react-icons/pi";

const EventModal = ({ onClose, event, resources, setEvents }) => {
	const [isEditing, setIsEditing] = useState(false);
	const [isLoading, setIsLoading] = useState(false);
	const [editedEvent, setEditedEvent] = useState({
		...event,
		start: new Date(event.start),
		end: new Date(event.end)
	});

	const handleSave = async () => {
		setIsLoading(true);
		const token = await getSessionToken();
		const { error } = await updateActivity(event.id, editedEvent, token);
		
		if (error) {
			alert("Der skete en fejl under opdateringen");
			setIsLoading(false);
			return;
		}
		
		setIsLoading(false);
		onClose();
	};

	const handleDelete = async () => {
		const token = await getSessionToken();
		const { error } = await deleteActivity(event.id, token);
		
		if (error) {
			alert("Der skete en fejl under sletningen");
			return;
		}

		setEvents((prevEvents) => prevEvents.filter((e) => e.id !== event.id));
		onClose();
	};

	return createPortal(
		<motion.div
			className="w-screen h-screen top-0 absolute bg-black bg-opacity-50 backdrop-blur-sm z-10 flex items-center justify-center"
			onClick={onClose}
			initial={{ opacity: 0 }}
			animate={{ opacity: 1 }}
			exit={{ opacity: 0 }}
		>
			<motion.div
				className="bg-white w-1/2 rounded-lg shadow-2xl"
				onClick={(e) => e.stopPropagation()}
				initial={{ scale: 0.5 }}
				animate={{ scale: 1 }}
				transition={{ type: "spring", stiffness: 260, damping: 20 }}
				exit={{ scale: 0.5 }}
			>
				<div className="flex justify-between p-5 bg-blue-500 rounded-t-lg">
					{isEditing ? (
						<input
							type="text"
							value={editedEvent.title}
							onChange={(e) => setEditedEvent({ ...editedEvent, title: e.target.value })}
							className="text-xl font-bold bg-white px-2 rounded"
						/>
					) : (
						<h2 className="text-xl text-white font-bold">{event.title}</h2>
					)}
					<div className="flex gap-2">
						<button 
							onClick={() => setIsEditing(!isEditing)}
							className="text-white hover:text-gray-200"
						>
							<FiEdit size={20} />
						</button>
						<button onClick={onClose} className="text-white hover:text-gray-200">X</button>
						<button onClick={handleDelete} className="text-white hover:text-gray-200">
							<PiTrash size={20} />
						</button>
					</div>
				</div>

				<div className="p-5 space-y-4">
					<div className="space-y-2">
						<label className="font-bold block">Beskrivelse:</label>
						{isEditing ? (
							<textarea
								value={editedEvent.description}
								onChange={(e) => setEditedEvent({ ...editedEvent, description: e.target.value })}
								className="w-full p-2 border rounded"
								rows={4}
							/>
						) : (
							<p>{event.description}</p>
						)}
					</div>

					<div className="space-y-2">
						<div className="flex items-center space-x-2">
							<label className="font-bold">Heldags:</label>
							{isEditing ? (
								<input
									type="checkbox"
									checked={editedEvent.allDay}
									onChange={(e) => setEditedEvent({ ...editedEvent, allDay: e.target.checked })}
								/>
							) : (
								<span>{event.allDay ? "Ja" : "Nej"}</span>
							)}
						</div>

						<div className="flex items-center space-x-2">
							<label className="font-bold">Start:</label>
							{isEditing ? (
								<input
									type="datetime-local"
									value={editedEvent.start.toISOString().slice(0, 16)}
									onChange={(e) => setEditedEvent({ ...editedEvent, start: new Date(e.target.value) })}
									className="border rounded p-1"
								/>
							) : (
								<span>{event.start.toLocaleString()}</span>
							)}
						</div>

						<div className="flex items-center space-x-2">
							<label className="font-bold">Slut:</label>
							{isEditing ? (
								<input
									type="datetime-local"
									value={editedEvent.end.toISOString().slice(0, 16)}
									onChange={(e) => setEditedEvent({ ...editedEvent, end: new Date(e.target.value) })}
									className="border rounded p-1"
								/>
							) : (
								<span>{event.end.toLocaleString()}</span>
							)}
						</div>
					</div>

					{isEditing && (
						<div className="flex justify-end pt-4">
							<button
								onClick={handleSave}
								className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 flex items-center gap-2"
								disabled={isLoading}
							>
								{isLoading ? (
									<PiSpinnerGap className="animate-spin" />
								) : (
									"Gem ændringer"
								)}
							</button>
						</div>
					)}
				</div>
			</motion.div>
		</motion.div>,
		document.getElementById("root")
	);
};

export default EventModal;

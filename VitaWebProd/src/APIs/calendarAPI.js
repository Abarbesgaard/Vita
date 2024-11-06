export const getAllActivities = async (token) => {
	
	const data = { activities: null, error: null };

	try {
		const response = await fetch("https://localhost:8081/api/activity/getall", {
			headers: {
				noCors: true,
				Authorization: "Bearer " + token,
			},
		}).then((response) => response.json());

		data.activities = response;
	} catch (error) {
		data.error = error;
	}
	return data;
};

export const saveActivity = async (activity, token) => {
	try {
		const response = await fetch("https://localhost:8081/api/activity/create", {
			method: "POST",
			headers: {
				Authorization: "Bearer " + token,
				"Content-Type": "application/json",
				charset: "utf-8",
			},
			body: JSON.stringify({
				start: activity.start,
				end: activity.end,
				hostId: activity.hostId,
				attendee: activity.attendee,
				cancelled: activity.cancelled,
				allDayEvent: activity.allDayEvent,
				verifiedAttendee: [],
				title: activity.title,
				description: activity.description,
			}),
		}).then((response) => response.json());
		console.log(response);
		return response;
	} catch (error) {
		return error;
	}
};

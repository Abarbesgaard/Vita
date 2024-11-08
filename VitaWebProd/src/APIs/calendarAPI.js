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
export const createActivity = async (activity, token) => {
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

export const updateActivity = async (id, activity, token) => {
    try {
        const API_URL = 'http://localhost:5000';
        
        const response = await fetch(`${API_URL}/api/Activity/Update/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                id: activity.id,
                title: activity.title,
                description: activity.description,
                start: activity.start.toISOString(),
                end: activity.end.toISOString(),
                allDayEvent: activity.allDay,
                hostId: activity.hostId,
                attendee: activity.attendee,
                verifiedAttendee: activity.accepted,
                cancelled: activity.cancelled
            })
        });

        if (!response.ok) {
            const errorData = await response.text();
            console.error('Server response:', errorData);
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return { activity: data, error: null };
    } catch (error) {
        console.error('Error updating activity:', error);
        return { activity: null, error: error.message };
    }
};

export const deleteActivity = async (id, token) => {
    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Activity/Delete/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return { error: null };
    } catch (error) {
        console.error('Error deleting activity:', error);
        return { error: error.message };
    }
};


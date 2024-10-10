import React, { useState, useEffect } from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import 'react-big-calendar/lib/css/react-big-calendar.css';
import EventCard from '../../Components/EventCard';

const myEventsList = [
    {
        title: 'Film aften',
        description: 'Vita aftensmad, i aften skal vi hygge med filmen biler og spise bolognese',
        start: new Date(2024, 9, 10, 10, 0), // Year, Month (0-indexed), Day, Hour, Minute
        end: new Date(2024, 9, 10, 12, 0),
    },
    {
        title: 'Event 2',
        description: 'Description for Event 2',
        start: new Date(2024, 9, 11, 14, 0),
        end: new Date(2024, 9, 11, 16, 0),
    },
];

const localizer = momentLocalizer(moment);

const MyCalendar = () => {
    const [events, setEvents] = useState(myEventsList);
    const [showCard, setShowCard] = useState(false);
    const [selectedSlot, setSelectedSlot] = useState(null);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [showEventDetails, setShowEventDetails] = useState(false);

    const handleAddEvent = () => {
        setShowCard(true);
        setSelectedSlot(new Date());
        setSelectedEvent(null);
    };

    const handleSaveEvent = (newEvent) => {
        if (selectedEvent) {
            // Modify existing event
            const updatedEvents = events.map(event =>
                event === selectedEvent ? { ...event, ...newEvent } : event
            );
            setEvents(updatedEvents);
        } else {
            // Add new event
            setEvents([...events, newEvent]);
        }

        setShowCard(false);
        setSelectedSlot(null);
        setSelectedEvent(null);
    };

    const handleSelectEvent = (event) => {
        setSelectedEvent(event);
        setShowEventDetails(true);
    };

    const handleSelectSlot = (slotInfo) => {
        setShowCard(true);
        setSelectedSlot(slotInfo);
        setSelectedEvent(null);
    };

    const handleDeleteEvent = () => {
        const updatedEvents = events.filter(event => event !== selectedEvent);
        setEvents(updatedEvents);
        setShowEventDetails(false);
        setSelectedEvent(null);
    };

    const handleCloseEventDetails = () => {
        setShowEventDetails(false);
        setSelectedEvent(null);
    };

    const handleModifyEvent = () => {
        setShowCard(true);
        setShowEventDetails(false);
    };

    return (
        <div>
            <div style={{ height: '600px', overflow: 'hidden' }}>
                <Calendar
                    localizer={localizer}
                    events={events}
                    startAccessor="start"
                    endAccessor="end"
                    style={{ height: '100%' }}
                    view='week'
                    views={[ 'month', 'week']}
                    onSelectEvent={handleSelectEvent}
                    onSelectSlot={handleSelectSlot}
                    selectable={true}
                    min={new Date(0, 0, 0, 6, 0, 0)} // Set minimum time to 6:00
                    max={new Date(0, 0, 0, 23, 0, 0)} // Set maximum time to 23:00
                    formats={{
                        timeGutterFormat: (date, culture, localizer) =>
                            localizer.format(date, 'HH:mm', culture),
                        eventTimeRangeFormat: ({ start, end }, culture, localizer) =>
                            localizer.format(start, 'HH:mm', culture) + ' - ' +
                            localizer.format(end, 'HH:mm', culture)
                    }}
                />
                <button onClick={handleAddEvent}>Opret Event</button>
            </div>
            <EventCard 
                show={showCard}
                onClose={() => setShowCard(false)}
                onSave={handleSaveEvent}
                selectedSlot={selectedSlot}
                event={selectedEvent}
            />
            {showEventDetails && selectedEvent && (
                <div className="modal-overlay">
                    <div className="modal-popup">
                        <h2>{selectedEvent.title}</h2>
                        <p><strong>Description:</strong> {selectedEvent.description}</p>
                        <p><strong>Start:</strong> {moment(selectedEvent.start).format('MMMM Do YYYY, h:mm a')}</p>
                        <p><strong>End:</strong> {moment(selectedEvent.end).format('MMMM Do YYYY, h:mm a')}</p>
                        <div className="button-group">
                            <button onClick={handleModifyEvent} className="btn btn-save">Modify</button>
                            <button onClick={handleDeleteEvent} className="btn btn-cancel">Delete</button>
                            <button onClick={handleCloseEventDetails} className="btn">Close</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MyCalendar;
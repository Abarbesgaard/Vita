import React, { useState } from 'react';
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

    const handleAddEvent = () => {
        setShowCard(true);
        setSelectedSlot(new Date());
    };

    const handleSaveEvent = (newEvent) => {
        setEvents([...events, newEvent]);

        setShowCard(false);
        setSelectedSlot(null);
        setSelectedEvent(null);
    };

    const handleSelectEvent = (event) => {
        setSelectedEvent(event);
    };

    const handleSelectSlot = (slotInfo) => {
        setShowCard(true);
        setSelectedSlot(slotInfo);
    };

    const handleDeleteEvent = (eventToDelete) => {
        const updatedEvents = events.filter(event => 
            event.start !== eventToDelete.start || 
            event.end !== eventToDelete.end || 
            event.title !== eventToDelete.title
        );
        setEvents(updatedEvents);
    };

    const handleCloseEventDetails = () => {
        setSelectedEvent(null);
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
                onSave={(newEvent) => {
                    setEvents([...events, newEvent]);
                    setShowCard(false);
                }}
                selectedSlot={selectedSlot}
            />
        </div>
    );
};

export default MyCalendar;
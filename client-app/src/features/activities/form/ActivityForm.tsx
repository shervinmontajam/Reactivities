import React, { useState, FormEvent, useContext } from 'react'
import { Segment, Form, Button } from 'semantic-ui-react'
import { IActivity } from '../../../app/models/activity'
import { v4 as uuid } from 'uuid';
import ActivityStore from '../../../app/stores/activityStore';

interface IProps {
    selectedActivity: IActivity | null;
}

const ActivityForm: React.FC<IProps> = ({ selectedActivity }) => {

    const activityStore = useContext(ActivityStore);
    const { createActivity, editActivity, submitting, cancelOpenForm } = activityStore;

    const initialForm = () => {
        if (selectedActivity) {
            return selectedActivity;
        }
        else {
            return {
                id: '',
                title: '',
                description: '',
                category: '',
                date: '',
                city: '',
                venue: ''
            }
        }
    }

    const [activity, setActivity] = useState<IActivity>(initialForm);

    const handleInputChange = (event: FormEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = event.currentTarget;
        setActivity({ ...activity, [name]: value });
    }

    const handleSubmit = () => {
        if (activity.id.length === 0) {
            let newActivity = {
                ...activity,
                id: uuid()
            }
            createActivity(newActivity);
        } else {
            editActivity(activity);
        }
    }

    return (
        <div>
            <Segment clearing>
                <Form onSubmit={() => handleSubmit()}>
                    <Form.Input name="title" placeholder="Title" value={activity.title} onChange={(event) => handleInputChange(event)} />
                    <Form.TextArea name="description" rows={2} placeholder="Description" value={activity.description} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="category" placeholder="Category" value={activity.category} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="date" placeholder="Date" type="datetime-local" value={activity.date} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="city" placeholder="City" value={activity.city} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="venue" placeholder="Venue" value={activity.venue} onChange={(event) => handleInputChange(event)} />
                    <Button loading={submitting} floated="right" positive type="submit" content="Submit" />
                    <Button floated="right" type="button" content="Cancel" onClick={() => cancelOpenForm()} />
                </Form>
            </Segment>
        </div>
    )
}

export default ActivityForm;

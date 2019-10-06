import React, { useState, FormEvent, useContext, useEffect } from 'react'
import { Segment, Form, Button, Grid } from 'semantic-ui-react'
import { IActivity } from '../../../app/models/activity'
import { v4 as uuid } from 'uuid';
import ActivityStore from '../../../app/stores/activityStore';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';

interface DetailParams {
    id: string
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({ match, history }) => {

    const activityStore = useContext(ActivityStore);
    const { createActivity, editActivity, submitting, activity: selectedActivity, loadActivity, clearActivity } = activityStore;



    const [activity, setActivity] = useState<IActivity>({
        id: '',
        title: '',
        description: '',
        category: '',
        date: '',
        city: '',
        venue: ''
    });

    useEffect(() => {
        if (match.params.id && activity.id.length === 0) {
            loadActivity(match.params.id)
                .then(() => {
                    selectedActivity && setActivity(selectedActivity);
                });
        }

        return () => {
            clearActivity();
        }
    }, [loadActivity, clearActivity, selectedActivity, match.params.id, activity.id.length]);

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
            createActivity(newActivity).then(()=>{
                history.push(`/activities/${newActivity.id}`);
            });
        } else {
            editActivity(activity).then(()=>{
                history.push(`/activities/${activity.id}`);
            });
        }
    }

    return (
        <Grid>
            <Grid.Column width={10}>
            <Segment clearing>
                <Form onSubmit={() => handleSubmit()}>
                    <Form.Input name="title" placeholder="Title" value={activity.title} onChange={(event) => handleInputChange(event)} />
                    <Form.TextArea name="description" rows={2} placeholder="Description" value={activity.description} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="category" placeholder="Category" value={activity.category} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="date" placeholder="Date" type="datetime-local" value={activity.date} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="city" placeholder="City" value={activity.city} onChange={(event) => handleInputChange(event)} />
                    <Form.Input name="venue" placeholder="Venue" value={activity.venue} onChange={(event) => handleInputChange(event)} />
                    <Button loading={submitting} floated="right" positive type="submit" content="Submit" />
                    <Button floated="right" type="button" content="Cancel" onClick={() => history.push("/activities")} />
                </Form>
            </Segment>
            </Grid.Column>
        </Grid>
    )
}

export default observer(ActivityForm);

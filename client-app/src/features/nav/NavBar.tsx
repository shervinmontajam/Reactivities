import React, { useContext } from 'react';
import { Menu, Container, Button } from 'semantic-ui-react';
import ActivityStore from '../../app/stores/activityStore';
import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';

const NavBar : React.FC = () => {

  const activityStore = useContext(ActivityStore);
  const { openCreateForm } = activityStore;

  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item header as={NavLink} to="/" exact>
          <img src="/assets/logo.png" alt="logo" style={{ marginRight: "10px" }} />
          Reactivities
        </Menu.Item>
        <Menu.Item name='Activities' as={NavLink} to="/activities" exact />
        <Menu.Item>
          <Button as={NavLink} to="createActivity" exact positive content="Create Activity" onClick={() => openCreateForm()} />
        </Menu.Item>
      </Container>
    </Menu>
  )
}

export default observer(NavBar);

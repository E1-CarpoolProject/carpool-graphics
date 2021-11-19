from mesa import Agent, Model
from mesa.time import RandomActivation
import numpy as np

positions = [[1,1],[1,2],[1,3],[2,1],[2,2],[2,3]]

class MoneyAgent(Agent):
    """ An agent with fixed initial wealth."""
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.wealth = 1

    def step(self):
        # The agent's step will go here.
        print ("Hi, I am agent " + str(self.unique_id) +".")
        if self.wealth == 0:
            return
        other_agent = self.random.choice(self.model.schedule.agents)
        other_agent.wealth += 1
        self.wealth -= 1

class PassengerAgent(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        #posiciones de origen y destino random
        a = np.random.randint(len(positions))
        while True:
            b = np.random.randint(len(positions))
            if a != b:
                break
        self.x_pos = positions[a][0]
        self.y_pos = positions[a][1]
        self.y_destination = positions[b][1]
        self.x_destination = positions[b][0]
        self.is_traveling = False
        self.has_arrived = False

    def imprimirPos(self):
        if self.is_traveling:
            print("estoy viajando")
        elif self.has_arrived:
            print("ya llegue")
        else:
            print("estoy esperando ride")
    def needs_ride(self):
        return not self.is_traveling or not self.has_arrived

    def arrived_destination(self):
        return self.has_arrived

    def step(self):
        # The agent's step will go here.
        print ("pasajero número " + str(self.unique_id))
        print( "Mi posicion de origen es: "+str(self.x_pos)+", "+ str(self.y_pos)+ ". ")
        print( "Mi posicion de destino es: "+str(self.x_destination)+", "+ str(self.y_destination)+ ". ")
        self.imprimirPos()

class PassengerModel(Model):
    """A model with some number of agents."""
    def __init__(self, N):
        self.num_agents = N
        self.schedule = RandomActivation(self)
        # Create agents
        for i in range(self.num_agents):
            a = PassengerAgent(i, self)
            self.schedule.add(a)

    def step(self):
        '''Advance the model by one step.'''
        self.schedule.step()

class CarAgent(Agent):
    """ An agent with fixed initial wealth."""
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        a = np.random.randint(len(positions))
        while True:
            b = np.random.randint(len(positions))
            if a != b:
                break
        self.x_pos = positions[a][0]
        self.y_pos = positions[a][1]
        self.y_destination = positions[b][1]
        self.x_destination = positions[b][0]
        self.x_next_stop = 0
        self.y_next_stop = 0
        self.occupancy = 1
        self.capacity = 5
        # REVISAR
        self.passengers = []

    def step(self):
        # The agent's step will go here.
        print ("Car " + str(self.unique_id) +".")

    def drive_unit():
        return True

    def update_route():
        return True

    def arrive_at_destination():
        return self.x_pos == self.x_destination and self.y_pos == self.y_destination
    def pick_up():
        if self.occupancy < self.capacity:
            self.occupancy += 1
            return True
        else:
            print("El coche está lleno :(")
            return False
class CarModel(Model):
    """A model with some number of agents."""
    def __init__(self, N):
        self.num_agents = N
        self.schedule = RandomActivation(self)
        # Create agents
        for i in range(self.num_agents):
            a = CarAgent(i, self)
            self.schedule.add(a)

    def step(self):
        '''Advance the model by one step.'''
        self.schedule.step()

class TrafficLightAgent(Agent):
    """ An agent with fixed initial wealth."""
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.intersection_id = 1
        # pensaba 1) z, 2) -z, 3) x, 4) -x
        """
            _______|2   |________
                         4
            _____3_     ________
                   |   1|
        """
        self.direction = 1
        self.status = 0
        #Pensaba tipo 0-rojo, 1-amarillo, 2-verde

    def can_pass(self):
        if self.status==2:
            return True
        else:
            return False

    def step(self):
        # The agent's step will go here.
        print ("TrafficLightAgent " + str(self.unique_id) +".")


class TrafficLightModel(Model):
    """A model with some number of agents."""
    def __init__(self, N):
        self.num_agents = N
        self.schedule = RandomActivation(self)
        # Create agents
        for i in range(self.num_agents):
            a = TrafficLightAgent(i, self)
            self.schedule.add(a)

    def step(self):
        '''Advance the model by one step.'''
        self.schedule.step()

class IntersectionAgent(Agent):
    """ An agent with fixed initial wealth."""
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        #esto lo pensaba para calcular nuestros 3 puntos y se de la vuelta bonito
        self.x_prev = 0
        self.y_prev = 0
        self.x_final = 0
        self.y_final = 0
        self.x_pos = 0
        self.y_pos = 0
        self.traffic_light = []


    def change_traffic_light_status(self):
        print("quelque chose")
        return True

    def points_turn(self, direction):
        if direction == 1:
            self.x_prev = self.x_pos + 1
            self.y_prev = self.y_pos - 1
        elif direction == 2:
            self.x_prev = self.x_pos - 1
            self.y_prev = self.y_pos + 1
        elif direction == 3:
            self.x_prev = self.x_pos - 1
            self.y_prev = self.y_pos - 1
        elif direction == 4:
            self.x_prev = self.x_pos + 1
            self.y_prev = self.y_pos + 1
# pensaba 1) z, 2) -z, 3) x, 4) -x (z=y)
"""
     z
     |
-x ---- x
     |
     -z

    _______|2   |________
                 4
    _____3_     ________
           |   1|
"""
    def step(self):
        # The agent's step will go here.
        print ("TrafficLightAgent " + str(self.unique_id) +".")
class MoneyModel(Model):
    """A model with some number of agents."""
    def __init__(self, N):
        self.num_agents = N
        self.schedule = RandomActivation(self)
        # Create agents
        for i in range(self.num_agents):
            a = MoneyAgent(i, self)
            self.schedule.add(a)

    def step(self):
        '''Advance the model by one step.'''
        self.schedule.step()

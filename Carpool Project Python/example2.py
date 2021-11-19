from example import *


#inicializa 5 pasajeros
passenger_model = PassengerModel(5)
passenger_model.step()

#inicializa 5 coches
car_model = CarModel(5)
car_model.step()

traffic_light_model = TrafficLightModel(5)
traffic_light_model.step()

# para recorrer la lista de agentes
for agent in passenger_model.schedule.agents:
    print(agent.needs_ride())

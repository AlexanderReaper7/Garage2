# Garage 2.0

## [Övning 12: Garage 2.0 - del 1](/Övning12_Garage2_del1.pdf)

## [Övning 12: Garage 2.0 - del 2](/Övning12_Garage2_del2.pdf)

## [Övning 14: Garage 3.0](/Övning14_Garage3.0.pdf)

## User stories

Then person using this website is the "Parking attendant" for the parking lot aka user.
The Parking attendant can add/remove members, add/remove vehicles, and see the current state of the parking lot Etc.

When the user enters the website, the index is shown with the members, parked vehicles, and the parking spots. And buttons to add members and park vehicles.

When the user wants to add a new member, the user clicks on the "add member" button and a form is shown to create a new member.
The user gets redirected to the add vehicle form to optionally register a vehicle, once completed the vehicle gets added to the member.

When registering/creating a vehicle, a list of members is shown to select a member to register the vehicle to, the user selects a member and is redirected to form with the member pre filled in.

When deregistering/deleting a vehicle the vehicle is deregistered from the member the the ParkedVehicle is deleted.

When unparking a vehicle the vehicle is not deleted but simply removed from the parking spot and the ParkingSpace property becomes 0.

When parking a vehicle the ParkingLotManager class figures out where to parke the vehicle and sets the ParkingSpace and ParkingSubSpace property to the parking spot number.

The Details view for the Member shows the member´s regisered vehicles.

The Details view for the ParkedVehicle shows the member the vehicle is registered to.

<details>
<summary>Other Group's Repositories</summary>
- [Group 1](https://github.com/SushmaSrinivasan/Garage-3.0-group-1)
- [Group 3](https://github.com/moon1204am/garage-3.0)
- [Group 4](https://github.com/josukattoor/Garage-3-MVCEF)
- [Group 5](https://github.com/samuellidstrom/Garage2.0_Group5)
- [Group 6](https://github.com/Kasleets/Garage3)
- [Group 7](https://github.com/EliasRafo/Garage3)
- [Group 8](https://github.com/dornax/Ovning_14)
</details>

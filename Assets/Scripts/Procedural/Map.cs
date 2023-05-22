using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map
{
    private List<Room> rooms;
    private int[] matrixRooms;
    private static readonly float SIZE_OF_ROOM_X = 19.0f;
    private static readonly float SIZE_OF_ROOM_Y = 19.0f;
    public static readonly float W_PIXELS = 3.2f;
    public static readonly float H_PIXELS = 3.2f;
    public static readonly int SIZE_OF_CHUNK = 10;

    public Map(List<Room> rooms, int[] matrixRooms)
    {
        AddRooms(rooms, matrixRooms);
    }

    public Map()
    {
        this.rooms = new List<Room>();
    }

    public void AddRooms(List<Room> rooms, int[] matrixRooms)
    {
        this.rooms = rooms;
        this.matrixRooms = matrixRooms;
        newRoomsPositions();
    }

    public List<Room> GetRooms()
    {
        return this.rooms;
    }

    public Room GetRoom(int index)
    {
        return rooms[index];
    }

    //codigo para buscar dados da matriz vezes a constante de tamanho
    private void newRoomsPositions()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            List<Vector3> roomsLocations = new List<Vector3>();
            int matrixIndex = matrixRooms[i];
            //Debug.Log("MATRIX: " + matrixIndex);
            Room room = rooms[i];
            foreach (Vector3 pos in room.GetPositions())
            {
                roomsLocations.Add(CalculateGlobalPosition(matrixIndex, pos));
            }
            room.UpdatePositions(roomsLocations);
            rooms[i] = room;
        }
    }

    /**
     * Calculo global de posições para uma cordenada matriz de elementos com cada item
     * sendo uma sala no mundo
     */
    private Vector3 CalculateGlobalPosition(int m_index, Vector3 position)
    {
        double row = (m_index / 4) * SIZE_OF_ROOM_X;
        double column = (m_index % 4) * SIZE_OF_ROOM_Y;
        position.x = (float)ToIsometricX(row,column) + position.x;
        position.y = (float)ToIsometricY(row,column) + position.y;
        return position;
    }

    /**
     * Convertendo Coordenadas
     */
    private double ToIsometricX(double x, double y)
    {
        double nx = (x * 0.5 * (W_PIXELS)) + (y * -0.5 * (W_PIXELS));
        return nx;
    }

    private double ToIsometricY(double x, double y)
    {
        double ny = (x * 0.25 * (H_PIXELS)) + (y * 0.25 * (H_PIXELS));
        return ny;
    }

}

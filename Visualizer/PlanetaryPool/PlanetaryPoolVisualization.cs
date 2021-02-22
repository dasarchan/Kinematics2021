﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using Visualizer.Kinematics;
using VisualizerBaseClasses;
using VisualizerControl;
using VisualizerControl.Commands;
using VisualizerControl.Shapes;
using static WPFUtility.UtilityFunctions;

namespace Visualizer.PlanetaryPool
{
    public class PlanetaryPoolVisualization : IVisualization
    {
        private GravitationalStructure gravStruct;
        private IEngine engine;
        private List<IProjectile> projectiles;
        private int counter = 0;

        public PlanetaryPoolVisualization(GravitationalStructure structure, IEngine engine)
        {
            gravStruct = structure;
            this.engine = engine;
            projectiles = engine.Projectiles;
        }

        public bool Continue { get; private set; }

        public double Time => engine.Time;

        public CommandSet<VisualizerControl.Visualizer> Initialization()
        {
            var set = new VisualizerCommandSet();

            var projectile = projectiles[0];
            // Start it off in the right place
            var obj = new ObjectPrototype(projectile.Shape, new BasicMaterial(projectile.Color, .05, .3),
                projectile.Position, new Vector3D(projectile.Size, projectile.Size, projectile.Size));

            set.AddCommand(new AddObject(obj, counter++));

            foreach (var tetrahedron in gravStruct.GetTetrahedra())
            {
                var newShape = new Tetrahedron3D(ConvertToVector3D(tetrahedron.Points[0]),
                    ConvertToVector3D(tetrahedron.Points[1]), ConvertToVector3D(tetrahedron.Points[2]),
                    ConvertToVector3D(tetrahedron.Points[3]));
                var newMaterial = new BasicMaterial(ConvertColor(tetrahedron.Color), tetrahedron.Fresnel, tetrahedron.Roughness);
                var newobj = new ObjectPrototype(newShape, newMaterial);

                set.AddCommand(new AddObject(newobj, counter++));
            }

            return set;
        }

        public CommandSet<VisualizerControl.Visualizer> Tick(double newTime)
        {
            Continue = engine.Tick(newTime);

            var set = new VisualizerCommandSet();

            set.AddCommand(new MoveObject(0, projectiles[0].Position));

            return set;
        }
    }
}

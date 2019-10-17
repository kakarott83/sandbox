using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;



namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse für SortTreeBo
    /// </summary>
    [TestFixture()]
    public class SortTreeBoTest
    {
       
        /// <summary>
        /// Initialisiert alle generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void SortTreeBoTestInit()
        {
           
        }
        
        /// <summary>
        /// Blackboxtest für die listAvailableServices Methode 
        /// </summary>
        [Test]
        public void flattenTreeTest()
        {
            SortTreeNode root = new SortTreeNode();
            root.sysid = 1;
            root.bezeichnung = "Wurzel";

            SortTreeNode child = new SortTreeNode();
            child.bezeichnung = "A";
            child.setParent(root);
            child.sysid = 2;
            root.addChild(child);

            SortTreeNode child2 = new SortTreeNode();
            child2.bezeichnung = "1";
            child2.setParent(child);
            child2.sysid = 4;
            child.addChild(child2);
            child2 = new SortTreeNode();
            child2.bezeichnung = "2";
            child2.setParent(child);
            child2.sysid = 5;
            child.addChild(child2);

            SortTreeNode child3 = new SortTreeNode();
            child3.bezeichnung = "a";
            child3.setParent(child2);
            child3.sysid = 10;
            child2.addChild(child3);
            child3 = new SortTreeNode();
            child3.bezeichnung = "b";
            child3.setParent(child2);
            child3.sysid = 11;
            child2.addChild(child3);

            child2 = new SortTreeNode();
            child2.bezeichnung = "3";
            child2.setParent(child);
            child2.sysid = 6;
            child.addChild(child2);

           
            child = new SortTreeNode();
            child.bezeichnung = "B";
            child.setParent(root);
            child.sysid = 3;
            root.addChild(child);
            child2 = new SortTreeNode();
            child2.bezeichnung = "1";
            child2.setParent(child);
            child2.sysid = 7;
            child.addChild(child2);
            child2 = new SortTreeNode();
            child2.bezeichnung = "2";
            child2.setParent(child);
            child2.sysid = 8;
            child.addChild(child2);

            SortTreeBo sbo = new SortTreeBo(new SortTreeDao());
            List<SortTreeNode> nodes = sbo.flattenTree(root);

            Assert.IsNotEmpty(nodes);
        }

        /// <summary>
        /// Blackboxtest für die listAvailableServices Methode 
        /// </summary>
        [Test]
        public void filterTreeTest()
        {
            SortTreeNode root = new SortTreeNode();
            root.sysid = -1;
            root.syssortpos = 1;
            root.syssortposp = -1;
            root.bezeichnung = "Wurzel";
            root.visible = true;

            SortTreeNode child = new SortTreeNode();
            child.bezeichnung = "A";
            child.setParent(root);
            child.sysid = 2;
            child.syssortpos = 11;
            child.syssortposp = 1;
            root.addChild(child);

            SortTreeNode child2 = new SortTreeNode();
            child2.bezeichnung = "1";
            child2.setParent(child);
            child2.sysid = 4;
            child2.syssortpos = 111;
            child2.syssortposp = 11;
            child.addChild(child2);

            child2 = new SortTreeNode();
            child2.bezeichnung = "2";
            child2.setParent(child);
            child2.sysid = 5;
            child2.syssortpos = 1111;
            child2.syssortposp = 111;
            child.addChild(child2);

            SortTreeNode child3 = new SortTreeNode();
            child3.bezeichnung = "a";
            child3.setParent(child2);
            child3.sysid = 10;
            child3.syssortpos = 11111;
            child3.syssortposp = 1111;
            child2.addChild(child3);

            child3 = new SortTreeNode();
            child3.bezeichnung = "b";
            child3.setParent(child2);
            child3.sysid = 11;
            child3.syssortpos = 111111;
            child3.syssortposp = 1111;
            child2.addChild(child3);

            child2 = new SortTreeNode();
            child2.bezeichnung = "3";
            child2.setParent(child);
            child2.sysid = 8;
            child2.syssortpos = 1111111;
            child2.syssortposp = 11;
            child.addChild(child2);


            child = new SortTreeNode();
            child.bezeichnung = "B";
            child.setParent(child2);
            child.sysid = 23445;
            child.syssortpos = 11111111;
            child.syssortposp = 1111111;
            child2.addChild(child);

            child2 = new SortTreeNode();
            child2.bezeichnung = "1";
            child2.setParent(child);
            child2.sysid =756;
            child2.syssortpos = 111111112;
            child2.syssortposp = 11111111;
            child.addChild(child2);

            child2 = new SortTreeNode();
            child2.bezeichnung = "2";
            child2.setParent(child);
            child2.sysid = 30;
            child2.syssortpos = 111111113;
            child2.syssortposp = 11111111;
            child.addChild(child2);


            List<FlatSortableDto> items = new List<FlatSortableDto>();
            FlatSortableDto item = new FlatSortableDto();
            item.sysid = 8;
            items.Add(item);
            item = new FlatSortableDto();
            item.sysid = 30;
            items.Add(item);
            item = new FlatSortableDto();
            item.sysid = 756;
            items.Add(item);

            SortTreeBo sbo = new SortTreeBo(new SortTreeDao());
            SortTreeNode nroot = sbo.filterTree(root,items);
            List<SortTreeNode> nodes = sbo.flattenTree(nroot);

            Assert.AreEqual("Wurzel", nodes[1].bezeichnung);
            //Assert.IsEmpty(nodes);
        }

    }
}